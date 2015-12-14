using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Nakladna.DAL
{
    internal enum ServiceCheckResult
    {
        Running,
        StartingError,
        NotFound
    }

    internal class SQLServiceChecker
    {
        internal event EventHandler SQLServiceStarting;
        internal event EventHandler SQLServiceStarted;

        private readonly ServiceController sqlService;
        private readonly string serviceName;

        internal SQLServiceChecker(string serviceName)
        {
            this.serviceName = serviceName;
            this.sqlService = new ServiceController(serviceName);
        }

        internal ServiceCheckResult CheckAndStartService()
        {
            ServiceControllerStatus status;
            try
            {
                status = sqlService.Status;
            }
            catch (Exception)
            {
                return ServiceCheckResult.NotFound;
            }

            if (status == ServiceControllerStatus.Stopped | status == ServiceControllerStatus.StopPending)
            {
                OnSqlServiceStarting();

                try
                {
                    sqlService.Start();
                    sqlService.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch (Exception)
                {
                    return ServiceCheckResult.StartingError;
                }

                OnSqlServiceStarted();
            }

            return ServiceCheckResult.Running;
        }

        protected virtual void OnSqlServiceStarting()
        {
            var handler = SQLServiceStarting;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnSqlServiceStarted()
        {
            var handler = SQLServiceStarted;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
