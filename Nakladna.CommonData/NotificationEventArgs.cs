using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public class NotificationEventArgs: EventArgs
    {
        public string Message { get; protected set; }

        public NotificationEventArgs(string message)
        {
            Message = message;
        }
    }
}
