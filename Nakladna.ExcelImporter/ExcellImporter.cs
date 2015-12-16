using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Nakladna.ExcelImporter
{
    public class ExcellImporter
    {
        public Dictionary<DateTime, IEnumerable<SaleParsed>> ImportFromFile(string path, GoodType type, string producer)
        {
            var result = new Dictionary<DateTime, IEnumerable<SaleParsed>>();

            using (var fs = File.OpenRead(path))
            {
                var workbook = new XSSFWorkbook(fs);

                for (int sheetIdx = 0; sheetIdx < workbook.NumberOfSheets; sheetIdx++)
                {

                    ISheet sheet = workbook.GetSheetAt(sheetIdx);

                    var datesRow = sheet.GetRow(Settings.DatesRow);

                    if (datesRow == null)
                        continue;

                    var datesCell = datesRow.GetCell(Settings.DatesColumn);

                    if (datesCell == null)
                        continue;

                    DateTime startDate = GetStartDate(datesCell);

                    var sales = ParseSales(sheet, datesCell, startDate, type, producer);
                    foreach (var s in sales)
                        result.Add(s.Key, s.Value);
                }
            }

            return result;
        }

        private DateTime GetStartDate(ICell datesCell)
        {
            try
            {
                return Settings.LastImportedDate.HasValue
                ? Settings.LastImportedDate.Value
                : datesCell.DateCellValue;
            }
            catch
            {
                return new DateTime(0);
            }
        }

        private Dictionary<DateTime, IEnumerable<SaleParsed>> ParseSales(ISheet sheet, ICell firstDateCell, DateTime startDate, GoodType type, string producer)
        {
            var sales = new Dictionary<DateTime, IEnumerable<SaleParsed>>();

            int customerColumn = Settings.CustomersColumn;
            int startRow = Settings.SalesStartRow;
            var datesEmptyLimit = 3;

            for (int c = firstDateCell.ColumnIndex; ; c++)
            {
                var dateCell = sheet.GetRow(firstDateCell.RowIndex).GetCell(c);

                if (dateCell == null)
                {
                    if (--datesEmptyLimit == 0)
                        break;
                    continue;
                }
                else
                {
                    datesEmptyLimit = 3;
                }

                if (dateCell.CellType != CellType.Numeric || dateCell.DateCellValue <= startDate)
                    continue;

                var dSales = ParseDailySale(sheet, dateCell, type, dateCell.ColumnIndex, customerColumn, startRow, producer);

                if (dSales.Any())
                    sales.Add(dSales.First().DateTime, dSales);
            }

            return sales;
        }

        private static IEnumerable<SaleParsed> ParseDailySale(ISheet sheet, ICell datesCell, GoodType type, int columnIndex, int customerColumn, int startRow, string producer)
        {
            var sales = new List<SaleParsed>();
            DateTime date;

            try
            {
                date = datesCell.DateCellValue;
                Settings.LastImportedDate = date;
            }
            catch (Exception ex)
            {
                return sales;
            }

            for (int r = startRow; ; r++)
            {
                var customerCell = sheet.GetRow(r).GetCell(customerColumn);
                if (customerCell == null)
                    break;

                var customer = customerCell.StringCellValue.Trim();
                if (customer == "сумма" || customer == "сума")
                    break;

                var sale = new SaleParsed();
                sale.GoodType = type;
                sale.DateTime = date;
                sale.Customer = customer;
                sale.Producer = producer;

                var qtyCell = sheet.GetRow(r).GetCell(columnIndex);
                if (qtyCell != null)
                {
                    if (qtyCell.CellType != CellType.Numeric)
                        continue;

                    sale.Quantity = (int)qtyCell.NumericCellValue;

                    if (sale.Quantity <= 0)
                        continue;
                }

                var retCell = sheet.GetRow(r).GetCell(columnIndex + 1);
                if (retCell != null)
                {
                    if (retCell.CellType == CellType.Numeric)
                        sale.Return = (int)retCell.NumericCellValue;
                }

                sales.Add(sale);
            }

            Settings.Save();

            return sales;
        }
    }
}
