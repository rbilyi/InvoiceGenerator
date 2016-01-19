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
        private static readonly object _lock = new object();

        private static volatile Repository _repository;
        private static InvoicesContext _context;

        protected Repository()
        {
            CheckSqlServices();
            _context = new InvoicesContext(Settings.ConnectionString);
        }

        public static Repository Instance
        {
            get
            {
                if (_repository != null) return _repository;

                lock (_lock)
                {
                    if (_repository == null)
                        _repository = new Repository();
                }
                return _repository;
            }
        }

        #endregion

        public event EventHandler<DbNotificationEventArgs> DbNotification;

        public void CheckSqlServices()
        {
            var checker = new SQLServiceChecker(Settings.SQLServiceName);
            checker.SQLServiceStarting += (s, e) => { InvokeDbNotification("SQLServer's service is stopped. Attempting to start."); };
            checker.SQLServiceStarted += (s, e) => { InvokeDbNotification("SQLServer's service is started."); };
            var status = checker.CheckAndStartService();

            switch (status)
            {
                case ServiceCheckResult.StartingError:
                    throw new ApplicationException("Error while starting sql service.");
                case ServiceCheckResult.NotFound:
                    throw new ApplicationException("Can't find SQL service:" + Settings.SQLServiceName);
                case ServiceCheckResult.Running:
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SaveGoodType(GoodType entity)
        {
            if (entity.Id == null)
                _context.GoodTypes.Add(entity);

            SaveChanges();
        }

        public T Get<T>(int id) where T : EntityBase
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : EntityBase
        {
            return _context.Set<T>();
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
                return _context.Set<T>();

            return _context.Set<T>().Where(predicate).ToList();
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

            var existed = _context.Sales.FirstOrDefault(e => e.DateTime == s.DateTime && e.Customer.Id == s.Customer.Id);
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
            _context.Set<T>().Add(t);

            if (saveChanges)
                _context.SaveChanges();
        }

        public void Delete<T>(T t) where T : EntityBase
        {
            _context.Set<T>().Remove(t);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected virtual void InvokeDbNotification(string message)
        {
            var handler = DbNotification;
            if (handler != null) handler(this, new DbNotificationEventArgs(message));
        }
    }
}
