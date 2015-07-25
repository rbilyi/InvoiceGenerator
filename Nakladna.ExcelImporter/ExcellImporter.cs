using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Nakladna.ExcelImporter
{
    public class ExcellImporter
    {
        public Dictionary<DateTime, IEnumerable<SaleParsed>> ImportFromFile(string path, GoodType type)
        {
            var result = new List<SaleParsed>();

            using (var fs = File.OpenRead(path))
            {
                var workbook = new XSSFWorkbook(fs);

                ISheet sheet = workbook.GetSheetAt(0);

                var datesCell = sheet.GetRow(Settings.DatesRow).GetCell(Settings.DatesColumn);

                DateTime startDate = GetStartDate(datesCell);

                return ParseSales(sheet, datesCell, startDate, type);
            }
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

        private Dictionary<DateTime, IEnumerable<SaleParsed>> ParseSales(ISheet sheet, ICell firstDateCell, DateTime startDate, GoodType type)
        {
            var sales = new Dictionary<DateTime, IEnumerable<SaleParsed>>();

            int customerColumn = Settings.CustomersColumn;
            int startRow = Settings.SalesStartRow;

            for (int c = firstDateCell.ColumnIndex; ; c++)
            {
                var dateCell = sheet.GetRow(firstDateCell.RowIndex).GetCell(c);

                if (dateCell == null)
                    break;

                if (dateCell.CellType == CellType.Blank)
                    continue;

                if (dateCell.DateCellValue <= startDate)
                    continue;

                var dSales = ParseDailySale(sheet, dateCell, type, dateCell.ColumnIndex, customerColumn, startRow);

                if (dSales.Any())
                    sales.Add(dSales.First().DateTime, dSales);
            }

            return sales;
        }

        private static IEnumerable<SaleParsed> ParseDailySale(ISheet sheet, ICell datesCell, GoodType type, int columnIndex, int customerColumn, int startRow)
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
                sale.Producer = Settings.Producer;

                var qtyCell = sheet.GetRow(r).GetCell(columnIndex);
                if (qtyCell != null)
                    sale.Quantity = (int)qtyCell.NumericCellValue;

                var retCell = sheet.GetRow(r).GetCell(columnIndex + 1);
                if (retCell != null)
                    sale.Return = (int)retCell.NumericCellValue;

                sales.Add(sale);
            }

            Settings.Save();

            return sales;
        }
    }
}
