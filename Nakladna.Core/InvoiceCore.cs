using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Innvoice.Generator;
using Nakladna.CommonData;
using Nakladna.DAL;
using Nakladna.DataSheetImporter;

namespace Nakladna.Core
{
    public class InvoiceCore
    {
        #region Singleton
        private static InvoiceCore _instance;
        protected InvoiceCore()
        {
            Repository.Instance.DbNotification += (s, e) => { OnInitializationNotification(e); };
        }

        public static InvoiceCore Instance
        {
            get
            {
                return _instance ?? (_instance = new InvoiceCore());
            }
        }
        #endregion Singleton

        public Action<GoodType, string, string> FileRenamed;
        public Action<GoodType, string> FileChanged;

        public int NewCustomers { get; set; }

        public event EventHandler<NotificationEventArgs> InitializationNotification;

        public IReadOnlyDictionary<int, string> GetSheets(string filePath)
        {
            return new ExcellImporter().GetSheets(filePath);
        }

        public IEnumerable<Invoice> GetInvoicesByDatesRange(DateTime startDate, DateTime endDate)
        {
            return GenerateInvoices(GetSales(startDate, endDate), Settings.Producer);
        }

        public IEnumerable<Sale> GetSales(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            if (startDate > endDate)
            {
                var t = endDate;
                endDate = startDate;
                startDate = t;
            }

            return new DataProvider().GetSales(startDate, endDate);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return new DataProvider().GetCustomers();
        }

        public void AddSpecialPrice(GoodType good, Customer client, double price)
        {
            new DataProvider().AddSpecialPrice(good, client, price);
        }

        public IEnumerable<Invoice> GenerateInvoicesByDate(DateTime date)
        {
            IEnumerable<Sale> sales = new DataProvider().GetSales(date);

            return GenerateInvoices(sales, Settings.Producer);
        }

        public void SaveInvoices(IEnumerable<Invoice> invoices, string path, string runningPath)
        {
            var generator = new InvoiceGenerator();
            generator.CreateInOneDocument(invoices, path, runningPath);
        }

        public IEnumerable<GoodType> GetGoods()
        {
            return new DataProvider().GetGoods();
        }

        public async Task<IEnumerable<GoodType>> GetGoodsAsync()
        {
            return await new DataProvider().GetGoodsAsync();
        }

        public void ExportToDoc(DateTime startDate, DateTime endDate, string savePath, string runningPath)
        {
            var inv = GetInvoicesByDatesRange(startDate, endDate);

            if (!inv.Any())
                throw new NoSalesException();

            SaveInvoices(inv, savePath, runningPath);
        }

        public IEnumerable<Sale> ImportSalesFromXLS(string path, IEnumerable<int> sheets, DateTime dateTime, bool saveToDb = true)
        {
            var goods = new DataProvider().GetGoods();
            var producer = Settings.Producer;
            var importer = new ExcellImporter();
            var sales = importer.ImportFromSheet(path, dateTime, sheets, goods, producer);

            var result = new List<Sale>();
            NewCustomers = 0;

            foreach (var sale in sales)
            {
                var salesByDay = sale.Value.Select(s => s.ToSale()).ToList();
                NewCustomers += salesByDay.Count(s => !s.Customer.Id.HasValue);

                if (saveToDb)
                    new DataProvider().AddSales(salesByDay);

                result.AddRange(salesByDay);
            }

            return result;
        }

        public void StartAutoUpdating(GoodType goodType, string filePath, string producer)
        {
            //var updater = new AutoDBUpdater(goodType, filePath, producer);

            //var updateCheck = updater.ChechFileModified(Settings.ExcellFilePath, Settings.LastExcellFileHash);

            //if (updateCheck.Item1)
            //    FileChanged(goodType, filePath); 

            //updater.FileChanged = (path) =>
            //{
            //    Settings.LastExcellFileHash = updateCheck.Item2;
            //    ImportSalesFromXLS(path, goodType, producer, true);
            //    FileChanged(goodType, filePath);
            //};

            //updater.FileRenamed = (path) =>
            //{
            //    Settings.ExcellFilePath = path;
            //    Settings.LastExcellFileHash = updateCheck.Item2;
            //    FileRenamed(goodType, filePath, path);
            //};

            //updater.StartWatchingFile();
        }

        private IEnumerable<Invoice> GenerateInvoices(IEnumerable<Sale> sales, string producer)
        {
            var list = new List<Invoice>();

            var dates = sales.Select(s => s.DateTime).Distinct();
            var customers = sales.Select(s => s.Customer).Distinct();

            foreach (var d in dates)
            {
                foreach (var c in customers)
                {
                    var salesByCustomer = sales.Where(s => s.Customer.Name == c.Name && s.DateTime.Date == d.Date);
                    if (!salesByCustomer.Any())
                        continue;

                    var specialPrices = GetSpecialPrices().Where(sp => sp.Customer == c);

                    var invoice = new Invoice();
                    invoice.Customer = c;
                    invoice.Producer = producer;
                    invoice.DateTime = d;
                    invoice.SoldItems = new List<SoldItem>();

                    foreach (var s in salesByCustomer)
                    {
                        var qt = (s.Quantity - s.Return) > 0 ? (s.Quantity - s.Return) : 0;
                        var sPrice = specialPrices.FirstOrDefault(sp => sp.GoodType == s.GoodType);

                        invoice.SoldItems.Add(new SoldItem(s.GoodType, qt, sPrice?.Price));
                    }

                    list.Add(invoice);
                }

            }
            return list;
        }

        public IEnumerable<SpecialPrice> GetSpecialPrices()
        {
            return new DataProvider().GetSpecialSales();
        }

        public void SaveGoodType(GoodType goodType)
        {
            new DataProvider().SaveEntity(goodType);
        }

        protected virtual void OnInitializationNotification(NotificationEventArgs e)
        {
            var handler = InitializationNotification;
            if (handler != null) handler(this, e);
        }

        public void RemoveGoodType(GoodType good)
        {
            new DataProvider().Remove(good);
        }

        public void SaveDataChanges()
        {
            new DataProvider().SaveEntitiesChanges();
        }

        public void ClearSales()
        {
            new DataProvider().ClearSales();
        }
    }
}
