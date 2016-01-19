using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nakladna.CommonData;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Nakladna.DataSheetImporter
{
    public class ExcellImporter
    {
        private IEnumerable<GoodType> goodTypes;
        private string producer;

        public Dictionary<int, string> GetSheets(string path)
        {
            var result = new Dictionary<int, string>();

            using (var fs = File.OpenRead(path))
            {
                var workbook = new XSSFWorkbook(fs);

                for (int sheetIdx = 0; sheetIdx < workbook.NumberOfSheets; sheetIdx++)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetIdx);
                    result.Add(sheetIdx, sheet.SheetName);
                }
            }

            return result;
        }

        public Dictionary<DateTime, IEnumerable<SaleParsed>> ImportFromSheet(string path, DateTime dateTime, IEnumerable<int> sheetNumbers, IEnumerable<GoodType> goodTypes, string producer)
        {
            this.goodTypes = goodTypes;
            this.producer = producer;
            var result = new Dictionary<DateTime, IEnumerable<SaleParsed>>();

            using (var fs = File.OpenRead(path))
            {
                var workbook = new XSSFWorkbook(fs);

                foreach (var sNum in sheetNumbers)
                {
                    ISheet sheet = workbook.GetSheetAt(sNum);

                    var sales = ParseSales(sheet, dateTime);
                    foreach (var s in sales)
                        result.Add(s.Key, s.Value);
                }
            }

            return result;
        }

        private Dictionary<DateTime, IEnumerable<SaleParsed>> ParseSales(ISheet sheet, DateTime dateTime)
        {
            var sales = new Dictionary<DateTime, IEnumerable<SaleParsed>>();

            int customerColumn = Settings.CustomersColumn;
            int startRow = Settings.SalesStartRow;

            foreach (var good in goodTypes)
            {
                var dSales = ParseGoodSale(sheet, good, dateTime, customerColumn, startRow);

                if (dSales.Any())
                    sales.Add(dSales.First().DateTime, dSales);
            }

            return sales;
        }

        private IEnumerable<SaleParsed> ParseGoodSale(ISheet sheet, GoodType good, DateTime dateTime, int customerColumn, int startRow)
        {
            var sales = new List<SaleParsed>();

            for (int r = startRow; ; r++)
            {
                var customerCell = sheet.GetRow(r).GetCell(customerColumn);
                if (customerCell == null)
                    break;

                var customer = customerCell.StringCellValue.Trim();

                var sale = new SaleParsed();
                sale.GoodType = good;
                sale.DateTime = dateTime;
                sale.Customer = customer;
                sale.Producer = producer;

                var qtyCell = sheet.GetRow(r).GetCell(good.ColumnInDocument);
                if (qtyCell != null)
                {
                    if (qtyCell.CellType != CellType.Numeric)
                        continue;

                    sale.Quantity = (int)qtyCell.NumericCellValue;

                    if (sale.Quantity <= 0)
                        continue;
                }

                var retCell = sheet.GetRow(r).GetCell(good.ColumnInDocument + 1);
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
