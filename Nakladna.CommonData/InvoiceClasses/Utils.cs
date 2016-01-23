using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public static class Utils
    {
        public static class Excell
        {
            public static int GetColumnNumber(string name)
            {
                int number = 0;
                int pow = 1;
                for (int i = name.Length - 1; i >= 0; i--)
                {
                    number += (name[i] - 'A' + 1) * pow;
                    pow *= 26;
                }

                return number;
            }

            public static string GetExcelColumnName(int columnNumber)
            {
                int dividend = columnNumber;
                string columnName = String.Empty;
                int modulo;

                while (dividend > 0)
                {
                    modulo = (dividend - 1) % 26;
                    columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                    dividend = (int)((dividend - modulo) / 26);
                }

                return columnName;
            }
        }
    }
}
