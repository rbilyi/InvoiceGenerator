using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Nakladna.CommonData;

namespace Nakladna.DAL
{
    public class Repository
    {
        #region repository
        private static object _lock = new object();

        private static Repository _repository;
        private static InvoicesContext context;

        protected Repository()
        {
            context = new InvoicesContext(Settings.ConnectionString);
        }

        public static Repository Instance
        {
            get
            {
                if (_repository == null)
                {
                    lock (_lock)
                    {
                        if (_repository == null)
                            _repository = new Repository();
                    }
                }
                return _repository;
            }
        }

        #endregion

        public T Get<T>(int id) where T : EntityBase
        {
            return context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : EntityBase
        {
            return context.Set<T>();
        }

        //public IEnumerable<Sale> GetSales(Expression<Func<Sale, bool>> predicate = null) 
        //{
        //    if (predicate == null)
        //        return context.Set<Sale>();

        //    return context.Set<Sale>().Where(predicate).Include(s => s.Customer).ToList();
        //}

        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> predicate = null) where T : EntityBase
        {
            if (predicate == null)
                return context.Set<T>();

            return context.Set<T>().Where(predicate).ToList();
        }

        public void AddSale(Sale s, bool saveChanges = true)
        {
            if (s.Customer.Id == null)
            {
                Add(s.Customer, saveChanges);
            }

            if (s.GoodType.Id == null)
            {
                Add(s.GoodType, saveChanges);
            }

            var existed = context.Sales.FirstOrDefault(e => e.DateTime == s.DateTime && e.Customer.Id == s.Customer.Id);
            if (existed != null)
            {
                existed.Quantity = s.Quantity;
                existed.Return = s.Return;
            }
            else
            {
                Add(s, saveChanges);
            }
        }

        private void Add<T>(T t, bool saveChanges = true) where T : EntityBase
        {
            context.Set<T>().Add(t);

            if (saveChanges)
                context.SaveChanges();
        }

        public void Delete<T>(T t) where T : EntityBase
        {
            context.Set<T>().Remove(t);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
