using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnerSettings = Nakladna.CommonData.Settings;

namespace Nakladna
{
    public static class Settings
    {
        public static void Save()
        {
            InnerSettings.Default.Save();
        }

        public static void Reload()
        {
            InnerSettings.Default.Reload();
        }

        public static void Reset()
        {
            InnerSettings.Default.Reset();
        }

        public static int CustomersRow
        {
            get { return InnerSettings.Default.StartRow; }
            set { InnerSettings.Default.StartRow = value; }
        }

        public static int CustomersColumn
        {
            get { return InnerSettings.Default.CustomersColumn; }
            set { InnerSettings.Default.CustomersColumn = value; }
        }

        public static string TemplateFileName
        {
            get
            {
                return InnerSettings.Default.TemplatePath;
            }
            set
            {
                InnerSettings.Default.TemplatePath = value;
            }
        }

        public static int StartRow
        {
            get
            {
                return InnerSettings.Default.StartRow;
            }
            set
            {
                InnerSettings.Default.StartRow = value;
            }
        }

        public static string Producer
        {
            get
            {
                return InnerSettings.Default.Producer;
            }
            set
            {
                InnerSettings.Default.Producer = value;
            }
        }

        public static string ConnectionString
        {
            get { return InnerSettings.Default.ConnectionString; }
            set { InnerSettings.Default.ConnectionString = value; }
        }

        public static string ExcellFilePath
        {
            get { return InnerSettings.Default.LastImportedXLS; }
            set { InnerSettings.Default.LastImportedXLS = value; }
        }

        public static string SQLServiceName
        {
            get { return InnerSettings.Default.SQLServiceName; }
            set { InnerSettings.Default.SQLServiceName = value; }
        }
    }
}
