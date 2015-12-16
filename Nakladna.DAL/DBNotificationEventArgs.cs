using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nakladna.CommonData;

namespace Nakladna.DAL
{
    public class DbNotificationEventArgs: NotificationEventArgs
    {
        public DbNotificationEventArgs(string message): base(message)
        {
        }
    }
}
