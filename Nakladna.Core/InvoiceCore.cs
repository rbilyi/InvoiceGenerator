using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public void SaveEntitiesChanges()
        {
            Repository.Instance.SaveChanges();
        }

        public IEnumerable<GoodType> GetGoods()
        {
            var rep = Repository.Instance;
            return rep.GetAll<GoodType>();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var rep = Repository.Instance;
            return rep.GetAll<Customer>();
        }

        public IEnumerable<Customer> GetCustomers(Expression<Func<Customer, bool>> predicate)
        {
            var rep = Repository.Instance;
            return rep.Get(predicate);
        }

        public Customer GetCustomer(Expression<Func<Customer, bool>> predicate)
        {
            return Repository.Instance.Get(predicate).FirstOrDefault();
        }

        public void AddSales(IEnumerable<Sale> sales)
        {
            foreach (var s in sales)
                Repository.Instance.AddSale(s, false);

            Repository.Instance.SaveChanges();
        }

        public IEnumerable<Invoice> GetInvoicesByDatesRange(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;
            if (startDate > endDate)
            {
                var t = endDate;
                endDate = startDate;
                startDate = t;
            }

            IEnumerable<Sale> sales = Repository.Instance
                .Get<Sale>(s => s.DateTime >= startDate && s.DateTime <= endDate);

            return GenerateInvoices(sales).ToList();
        }

        public IEnumerable<Invoice> GenerateInvoicesByDate(DateTime date)
        {
            IEnumerable<Sale> sales = Repository.Instance
                .Get<Sale>(s => s.DateTime.Day == date.Day
                && s.DateTime.Month == date.Month
                && s.DateTime.Year == date.Year);

            return GenerateInvoices(sales);
        }

        public void SaveInvoices(IEnumerable<Invoice> invoices, string path, string runningPath)
        {
            var generator = new InvoiceGenerator();
            generator.CreateInOneDocument(invoices, path, runningPath);
        }

        public void ExportToDoc(DateTime startDate, DateTime endDate, string savePath, string runningPath)
        {
            var inv = GetInvoicesByDatesRange(startDate, endDate);

            if (!inv.Any())
                throw new NoSalesException();

            SaveInvoices(inv, savePath, runningPath);
        }

        public IEnumerable<Sale> ImportSalesFromXLS(string path, GoodType type, string producer, bool saveToDb = true)
        {
            var importer = new DataSheetImporter.ExcellImporter();
            var sales = importer.ImportFromFile(path, type, producer);
            var result = new List<Sale>();
            NewCustomers = 0;

            foreach (var sale in sales)
            {
                var salesByDay = sale.Value.Select(s => s.ToSale()).ToList();
                NewCustomers += salesByDay.Count(s => !s.Customer.Id.HasValue);

                if (saveToDb)
                    AddSales(salesByDay);

                result.AddRange(salesByDay);
            }

            return result;
        }

        public void StartAutoUpdating(GoodType goodType, string filePath, string producer)
        {
            var updater = new AutoDBUpdater(goodType, filePath, producer);

            var updateCheck = updater.ChechFileModified(Settings.ExcellFilePath, Settings.LastExcellFileHash);

            if (updateCheck.Item1)
                FileChanged(goodType, filePath); 

            updater.FileChanged = (path) =>
            {
                Settings.LastExcellFileHash = updateCheck.Item2;
                ImportSalesFromXLS(path, goodType, producer, true);
                FileChanged(goodType, filePath);
            };

            updater.FileRenamed = (path) =>
            {
                Settings.ExcellFilePath = path;
                Settings.LastExcellFileHash = updateCheck.Item2;
                FileRenamed(goodType, filePath, path);
            };

            updater.StartWatchingFile();
        }

        private IEnumerable<Invoice> GenerateInvoices(IEnumerable<Sale> sales, string producer = null)
        {
            var list = new List<Invoice>();
            foreach (var s in sales)
            {
                list.Add(new Invoice()
                {
                    Customer = s.Customer,
                    Producer = (producer == null || string.IsNullOrEmpty(s.Producer)) ? Settings.Producer : s.Producer,
                    DateTime = s.DateTime,
                    Supplies = new Dictionary<GoodType, int>() 
					{
						{s.GoodType, s.Quantity}
					}
                });
            }

            return list;
        }

        protected virtual void OnInitializationNotification(NotificationEventArgs e)
        {
            var handler = InitializationNotification;
            if (handler != null) handler(this, e);
        }
    }
}
