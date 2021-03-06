﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Innvoice.Generator;
using Nakladna.CommonData;
using Nakladna.DataSheetImporter;

namespace Nakladna.Core
{
    public class InvoiceCore
    {
        #region Singleton
        private static InvoiceCore _instance;
        protected InvoiceCore()
        {
            //Repository.Instance.DbNotification += (s, e) => { OnInitializationNotification(e); };
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

        public IEnumerable<SheetItem> GetSheets(string filePath)
        {
            return new ExcellImporter().GetSheets(filePath);
        }

        public IEnumerable<Invoice> GetInvoicesByDatesRange(DbScope scope, DateTime startDate, DateTime endDate)
        {
            return GenerateInvoices(scope, GetSales(scope, startDate, endDate), Settings.Producer);
        }

        public IEnumerable<Sale> GetSales(DbScope scope, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            if (startDate > endDate)
            {
                var t = endDate;
                endDate = startDate;
                startDate = t;
            }

            return scope.DataProvider.GetSales(startDate, endDate);
        }

        public IEnumerable<Customer> GetCustomers(DbScope scope)
        {
            return scope.DataProvider.GetCustomers();
        }

        public void AddSpecialPrice(DbScope scope, GoodType good, Customer client, double price)
        {
            scope.DataProvider.AddSpecialPrice(good, client, price);
        }

        public IEnumerable<Invoice> GenerateInvoicesByDate(DbScope scope, DateTime date)
        {
            IEnumerable<Sale> sales = scope.DataProvider.GetSales(date);

            return GenerateInvoices(scope, sales, Settings.Producer);
        }

        public void SaveInvoices(DbScope scope, IEnumerable<Invoice> invoices, string path, string runningPath)
        {
            var generator = new InvoiceGenerator();
            generator.CreateInOneDocument(invoices, path, runningPath);
        }

        public IEnumerable<GoodType> GetGoods(DbScope scope)
        {
            return scope.DataProvider.GetGoods();
        }

        public Task<IEnumerable<GoodType>> GetGoodsAsync(DbScope scope)
        {
            return scope.DataProvider.GetAllAsync<GoodType>();
        }

        public void ExportToDoc(DbScope scope, DateTime startDate, DateTime endDate, string savePath, string runningPath)
        {
            var inv = GetInvoicesByDatesRange(scope, startDate, endDate);

            if (!inv.Any())
                throw new NoSalesException();

            SaveInvoices(scope, inv, savePath, runningPath);
        }

        public IEnumerable<Sale> ImportSalesFromXLS(DbScope scope, string path, IEnumerable<int> sheets, DateTime dateTime, bool saveToDb = true)
        {
            var goods = scope.DataProvider.GetGoods();
            var producer = Settings.Producer;
            var importer = new ExcellImporter();
            var sales = importer.ImportFromSheet(path, dateTime, sheets, goods, producer);

            var result = new List<Sale>();
            NewCustomers = 0;

            foreach (var sale in sales)
            {
                var salesByDay = sale.Value.Select(s => s.ToSale(scope)).ToList();
                NewCustomers += salesByDay.Count(s => !s.Customer.Id.HasValue);

                if (saveToDb)
                    scope.DataProvider.AddSales(salesByDay);

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

        private IEnumerable<Invoice> GenerateInvoices(DbScope scope, IEnumerable<Sale> sales, string producer)
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

                    var specialPrices = scope.DataProvider.GetSpecialPrices().Where(sp => sp.Customer == c);

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

        public IEnumerable<SpecialPrice> GetSpecialPrices(DbScope scope)
        {
            return scope.DataProvider.GetSpecialPrices();
        }

        public void SaveGoodType(DbScope scope, GoodType goodType)
        {
            scope.DataProvider.SaveEntity(goodType);
        }

        protected virtual void OnInitializationNotification(NotificationEventArgs e)
        {
            var handler = InitializationNotification;
            if (handler != null) handler(this, e);
        }

        public void RemoveGoodType(DbScope scope, GoodType good)
        {
            scope.DataProvider.Remove(good);
        }

        public void ClearSales(DbScope scope)
        {
            scope.DataProvider.ClearSales();
        }

        public async Task<IEnumerable<GoodType>> GetGoodsAsync(DbScope scope, bool includeDeleted = false)
        {
            return await scope.DataProvider.GetAllAsync<GoodType>(includeDeleted);
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(DbScope scope, DateTime date1, DateTime date2)
        {
            return await scope.DataProvider.GetSalesAsync(date1, date2);
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(DbScope scope, DateTime day)
        {
            return await scope.DataProvider.GetSalesAsync(day, day);
        }

        public async Task<IEnumerable<SpecialPrice>> GetSpecialPricesAsync(DbScope scope, bool includeDeleted = false)
        {
            return await scope.DataProvider.GetAllAsync<SpecialPrice>(includeDeleted);
        }
    }

    public class DbScope : IDisposable
    {
        internal DataProvider DataProvider { get; private set; }

        public DbScope()
        {
            DataProvider = new DataProvider();
        }

        public void Submit()
        {
            DataProvider.SaveChanges();
        }

        public void Dispose()
        {
            DataProvider.SaveChanges();
        }
    }
}
