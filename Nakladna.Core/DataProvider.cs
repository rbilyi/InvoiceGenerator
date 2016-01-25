using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;
using Nakladna.DAL;

namespace Nakladna.Core
{
    class DataProvider
    {
        public void SaveChanges()
        {
            Repository.Instance.SaveChanges();
        }

        public void Remove(EntityBase entity)
        {
            Repository.Instance.Delete(entity);
        }

        public IEnumerable<GoodType> GetGoods()
        {
            var rep = Repository.Instance;
            return rep.GetAll<GoodType>();
        }

        public Task<IEnumerable<GoodType>> GetGoodsAsync()
        {
            var rep = Repository.Instance;
            return rep.GetAllAsync<GoodType>();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var rep = Repository.Instance;
            return rep.GetAll<Customer>();
        }

        public IEnumerable<Customer> GetCustomers(Func<Customer, bool> predicate)
        {
            var rep = Repository.Instance;
            return rep.Get(predicate);
        }

        public Customer GetCustomer(Func<Customer, bool> predicate)
        {
            return Repository.Instance.Get(predicate).FirstOrDefault();
        }

        public void AddSales(IEnumerable<Sale> sales)
        {
            foreach (var s in sales)
                Repository.Instance.AddSale(s, false);

            Repository.Instance.SaveChanges();
        }

        public IEnumerable<Sale> GetSales(DateTime startDate, DateTime endDate)
        {
            return Repository.Instance
                .Get<Sale>(s => s.DateTime >= startDate && s.DateTime <= endDate);
        }

        public IEnumerable<Sale> GetSales(DateTime date)
        {
            return Repository.Instance
                .Get<Sale>(s => s.DateTime.Day == date.Day
                && s.DateTime.Month == date.Month
                && s.DateTime.Year == date.Year);
        }

        internal void AddSpecialPrice(GoodType good, Customer client, double price)
        {
            var spPrice = new SpecialPrice();
            spPrice.GoodType = good;
            spPrice.Customer = client;
            spPrice.Price = price;
            SaveEntity(spPrice);
        }

        internal void SaveEntity(EntityBase entity)
        {
            Repository.Instance.SaveEntity(entity);
        }

        internal void ClearSales()
        {
            var sales = Repository.Instance.GetAll<Sale>();
            foreach (var s in sales)
                s.IsDeleted = true;

            SaveChanges();
        }

        internal IEnumerable<SpecialPrice> GetSpecialSales()
        {
            return Repository.Instance.GetAll<SpecialPrice>();
        }
    }
}
