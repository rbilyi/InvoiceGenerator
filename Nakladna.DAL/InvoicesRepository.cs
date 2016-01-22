﻿using System;
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
        private static volatile InvoicesContext _context;

        protected Repository()
        {
            //CheckSqlServices();
            _context = new InvoicesContext(Settings.ConnectionString);
        }

        public static Repository Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_repository != null)
                        return _repository;

                    if (_repository == null)
                        _repository = new Repository();

                    return _repository;
                }
            }
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

            SaveChanges();
        }

        public T Get<T>(int id, bool includeDeleted = false) where T : EntityBase
        {
            return _context.Set<T>().FirstOrDefault(e => e.Id == id && (!e.IsDeleted || includeDeleted));
        }

        public IEnumerable<T> GetAll<T>(bool includeDeleted = false) where T : EntityBase
        {
            return _context.Set<T>().Where(e => !e.IsDeleted || includeDeleted).ToList();
        }

        public IEnumerable<T> Get<T>(Func<T, bool> predicate = null, bool includeDeleted = false) where T : EntityBase
        {
            if (predicate == null)
                return _context.Set<T>().Where(e => !e.IsDeleted || includeDeleted).ToList();

            return _context.Set<T>()
                .Where(e => !e.IsDeleted || includeDeleted).ToList().Where(predicate);
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
            _context.Set(t.GetType()).Add(t);

            if (saveChanges)
                _context.SaveChanges();
        }

        public void Delete<T>(T t) where T : EntityBase
        {
            //_context.Set(t.GetType()).Remove(t);
            t.IsDeleted = true;
            _context.SaveChanges();
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
