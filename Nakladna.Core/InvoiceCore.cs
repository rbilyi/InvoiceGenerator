using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Innvoice.Generator;
using Nakladna.CommonData;
using Nakladna.DAL;
using Nakladna.ExcelImporter;

namespace Nakladna.Core
{
    public class InvoiceCore
    {
        #region Singleton
        private static InvoiceCore _instance;
        protected InvoiceCore()
        {
        }

        public static InvoiceCore Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InvoiceCore();

                return _instance;
            }
        }
        #endregion Singleton
        public int NewCustomers { get; set; }
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

        public void SaveInvoices(IEnumerable<Invoice> invoices, string path)
        {
            var generator = new InvoiceGenerator();
            generator.CreateInOneDocument(invoices, path);
        }

        public void ExportToDoc(DateTime startDate, DateTime endDate, string savePath)
        {
            var inv = GetInvoicesByDatesRange(startDate, endDate);

            if (inv.Count() == 0)
                throw new NoSalesException();

            SaveInvoices(inv, savePath);
        }

        public IEnumerable<Sale> ImportSalesFromXLS(string path, GoodType type, string producer, bool saveToDb = true)
        {
            var importer = new ExcellImporter();
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

    }
}
