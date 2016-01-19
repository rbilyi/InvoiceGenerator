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
        public void SaveEntitiesChanges()
        {
            Repository.Instance.SaveChanges();
        }

        public void Remove(GoodType goodType)
        {
            Repository.Instance.Delete(goodType);
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

        internal void SaveEntitiesChanges(GoodType goodType)
        {
            Repository.Instance.SaveGoodType(goodType);
        }
    }
}
