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
    internal class DataProvider
    {
        Repository repository;

        public DataProvider()
        {
            repository = new Repository();
        }
        public void Remove(EntityBase entity)
        {
            repository.Delete(entity);
        }

        public IEnumerable<GoodType> GetGoods()
        {
            var rep = repository;
            return rep.GetAll<GoodType>();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            var rep = repository;
            return rep.GetAll<Customer>();
        }

        public IEnumerable<Customer> GetCustomers(Func<Customer, bool> predicate)
        {
            var rep = repository;
            return rep.Get(predicate);
        }

        public Customer GetCustomer(Func<Customer, bool> predicate)
        {
            return repository.Get(predicate).FirstOrDefault();
        }

        public void AddSales(IEnumerable<Sale> sales)
        {
            foreach (var s in sales)
                repository.AddSale(s, false);
        }

        public IEnumerable<Sale> GetSales(DateTime startDate, DateTime endDate)
        {
            return repository
                .Get<Sale>(s => s.DateTime >= startDate && s.DateTime <= endDate);
        }

        public IEnumerable<Sale> GetSales(DateTime date)
        {
            return repository
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
            repository.SaveEntity(entity);
        }

        internal void ClearSales()
        {
            var sales = repository.GetAll<Sale>();
            foreach (var s in sales)
                s.IsDeleted = true;
        }

        internal IEnumerable<SpecialPrice> GetSpecialPrices()
        {
            return repository.GetAll<SpecialPrice>();
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(DateTime startDate, DateTime endDate)
        {
            return await repository
                .GetAllAsync<Sale>(s => s.DateTime >= startDate && s.DateTime <= endDate);
        }

        public async Task<IEnumerable<Sale>> GetSalesAsync(DateTime date)
        {
            return await repository
                .GetAllAsync<Sale>(s => s.DateTime.Day == date.Day
                && s.DateTime.Month == date.Month
                && s.DateTime.Year == date.Year);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(bool includeDeleted = false) where T : EntityBase
        {
            return await repository.GetAllAsync<T>(includeDeleted);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, bool includeDeleted = false) where T : EntityBase
        {
            return await repository.GetAllAsync<T>(predicate, includeDeleted);
        }

        internal void SaveChanges()
        {
            repository.SaveChanges();
        }
    }
}
