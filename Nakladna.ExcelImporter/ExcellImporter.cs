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

        public Dictionary<DateTime, List<SaleParsed>> ImportFromSheet(string path, DateTime dateTime, IEnumerable<int> sheetNumbers, IEnumerable<GoodType> goodTypes, string producer)
        {
            this.goodTypes = goodTypes;
            this.producer = producer;
            var result = new Dictionary<DateTime, List<SaleParsed>>();

            using (var fs = File.OpenRead(path))
            {
                var workbook = new XSSFWorkbook(fs);

                foreach (var sNum in sheetNumbers)
                {
                    ISheet sheet = workbook.GetSheetAt(sNum);

                    var sales = ParseSales(sheet, dateTime);
                    foreach (var s in sales)
                    {
                        if (result.ContainsKey(s.Key))
                        {
                            result[s.Key].AddRange(s.Value);
                        }
                        else
                        {
                            result.Add(s.Key, s.Value);
                        }
                    }
                }
            }

            return result;
        }

        private Dictionary<DateTime, List<SaleParsed>> ParseSales(ISheet sheet, DateTime dateTime)
        {
            var sales = new Dictionary<DateTime, List<SaleParsed>>();

            int customerColumn = Settings.CustomersColumn;
            int startRow = Settings.StartRow;

            foreach (var good in goodTypes)
            {
                var dSales = ParseGoodSale(sheet, good, dateTime, customerColumn, startRow).ToList();

                if (dSales.Any())
                {
                    var date = dSales.First().DateTime;

                    if (sales.ContainsKey(date))
                    {
                        sales[date].AddRange(dSales);
                    }
                    else
                    {
                        sales.Add(dSales.First().DateTime, dSales);
                    }
                }
            }

            return sales;
        }

        private IEnumerable<SaleParsed> ParseGoodSale(ISheet sheet, GoodType good, DateTime dateTime, int customerColumn, int startRow)
        {
            var sales = new List<SaleParsed>();

            for (int r = startRow; ; r++)
            {
                var row = sheet.GetRow(r);
                if (row == null)
                    break;

                var customerCell = row.GetCell(customerColumn);
                if (customerCell == null)
                    break;

                var customer = customerCell.StringCellValue.Trim();

                int qty = 0, ret = 0;
                var qtyCell = sheet.GetRow(r).GetCell(good.ColumnInDocument - 1);
                if (qtyCell != null && qtyCell.CellType == CellType.Numeric
                    && qtyCell.NumericCellValue > 0)
                {
                    qty = (int)qtyCell.NumericCellValue;
                }

                if (good.HasReturn)
                {
                    var retCell = sheet.GetRow(r).GetCell(good.ReturnColumn.Value - 1);
                    if (retCell != null && retCell.CellType == CellType.Numeric
                        && retCell.NumericCellValue > 0)
                    {
                        ret = (int)retCell.NumericCellValue;
                    }
                }

                if (qty > 0 && qty > ret)
                {
                    var sale = new SaleParsed();
                    sale.GoodType = good;
                    sale.DateTime = dateTime;
                    sale.Customer = customer;
                    sale.Producer = producer;
                    sale.Quantity = qty;
                    sale.Return = ret;
                    sales.Add(sale);
                }
            }

            return sales;
        }
    }
}
