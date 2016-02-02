using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Nakladna.CommonData;

namespace Nakladna.DAL
{
    public class Repository
    {
        InvoicesContext _context;
        #region repository
        public Repository()
        {
            _context = new InvoicesContext(Settings.ConnectionString);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(bool includeDeleted = false) where T : EntityBase
        {
            return await _context.Set<T>().Where(e => !e.IsDeleted || (e.IsDeleted && includeDeleted)).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, bool includeDeleted = false) where T : EntityBase
        {
            return await _context.Set<T>().Where(e => !e.IsDeleted || (e.IsDeleted && includeDeleted)).Where(predicate).ToListAsync();
        }

        #endregion

        public event EventHandler<DbNotificationEventArgs> DbNotification;

        //public void CheckSqlServices()
        //{
        //    var checker = new SQLServiceChecker(Settings.SQLServiceName);
        //    checker.SQLServiceStarting += (s, e) => { InvokeDbNotification("SQLServer's service is stopped. Attempting to start."); };
        //    checker.SQLServiceStarted += (s, e) => { InvokeDbNotification("SQLServer's service is started."); };
        //    var status = checker.CheckAndStartService();

        //    switch (status)
        //    {
        //        case ServiceCheckResult.StartingError:
        //            throw new ApplicationException("Error while starting sql service.");
        //        case ServiceCheckResult.NotFound:
        //            throw new ApplicationException("Can't find SQL service:" + Settings.SQLServiceName);
        //        case ServiceCheckResult.Running:
        //            return;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        public void SaveEntity(EntityBase entity)
        {
            if (entity.Id == null)
                Add(entity);
        }

        public T Get<T>(int id, bool includeDeleted = false) where T : EntityBase
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id && (!e.IsDeleted || (e.IsDeleted && includeDeleted)));
        }

        public IEnumerable<T> GetAll<T>(bool includeDeleted = false) where T : EntityBase
        {
            return _context.Set<T>().Where(e => !e.IsDeleted || (e.IsDeleted && includeDeleted)).ToList();
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate = null, bool includeDeleted = false) where T : EntityBase
        {
            if (predicate == null)
                return _context.Set<T>().Where(e => !e.IsDeleted || (e.IsDeleted && includeDeleted)).ToList();

            return _context.Set<T>()
                .Where(e => !e.IsDeleted || (e.IsDeleted && includeDeleted)).ToList().Where(predicate);
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

            Add(s, saveChanges);
        }

        private void Add<T>(T t, bool saveChanges = true) where T : EntityBase
        {
            _context.Set(t.GetType()).Add(t);

            if (saveChanges)
                _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Delete<T>(T t) where T : EntityBase
        {
            EntityBase e = (EntityBase)_context.Set(t.GetType()).Find(t.Id);
            //_context.Set(t.GetType()).Remove(e);
            e.IsDeleted = true;
            _context.SaveChanges();
        }

        protected virtual void InvokeDbNotification(string message)
        {
            var handler = DbNotification;
            if (handler != null) handler(this, new DbNotificationEventArgs(message));
        }
    }
}
