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

        public static int DatesRow
        {
            get { return InnerSettings.Default.DatesRow; }
            set { InnerSettings.Default.DatesRow = value; }
        }

        public static int DatesColumn
        {
            get { return InnerSettings.Default.DatesColumn; }
            set { InnerSettings.Default.DatesColumn = value; }
        }

        public static int CustomersRow
        {
            get { return InnerSettings.Default.CustomersRow; }
            set { InnerSettings.Default.CustomersRow = value; }
        }

        public static int CustomersColumn
        {
            get { return InnerSettings.Default.CustomersColumn; }
            set { InnerSettings.Default.CustomersColumn = value; }
        }

        public static string LastImportedXLS
        {
            get
            {
                try
                {
                    return InnerSettings.Default.LastImportedXLS;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                InnerSettings.Default.LastImportedXLS = value;
            }
        }

        public static DateTime? LastImportedDate
        {
            get
            {
                try
                {
                    return InnerSettings.Default.LastImportedDate;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                InnerSettings.Default.LastImportedDate = value.Value;
            }
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

        public static int SalesStartRow
        {
            get
            {
                return InnerSettings.Default.SalesStartRow;
            }
            set
            {
                InnerSettings.Default.SalesStartRow = value;
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
    }
}
