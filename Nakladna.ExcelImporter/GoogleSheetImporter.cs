using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using Nakladna.CommonData;
using Google.Spreadsheets;
using NPOI.SS.UserModel;

namespace Nakladna.DataSheetImporter
{
    public class GoogleSheetImporter
    {
        private readonly SpreadsheetsService spreadsheetsService;

        public GoogleSheetImporter(string email, string pas)
        {
            spreadsheetsService = new SpreadsheetsService("invoiceGenerator");
            spreadsheetsService.setUserCredentials(email, pas);
        }

        public Dictionary<DateTime, IEnumerable<SaleParsed>> ImportFromFile(string fileName, GoodType type, string producer)
        {
            var result = new Dictionary<DateTime, IEnumerable<SaleParsed>>();

            var feed = GetSpreadSheetsFeed();

            var mySpreadsheet = (SpreadsheetEntry)feed.Entries.FirstOrDefault(e => e.Title.Text.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
            if (mySpreadsheet == null)
                throw new FileNotFoundException(fileName);

            //retrieve the worksheets of a particular spreadsheet
            var link = mySpreadsheet.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);
            var wQuery = new WorksheetQuery(link.HRef.ToString());
            var wFeed = spreadsheetsService.Query(wQuery);
            //retrieve the cells in a worksheet
            //var worksheetEntry = (WorksheetEntry)wFeed.Entries[0];

            foreach (var wSheetAtomEntry in wFeed.Entries)
            {
                var wSheet = (WorksheetEntry)wSheetAtomEntry;
                var cLink = wSheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);
                var cQuery = new CellQuery(cLink.HRef.ToString());
                var cFeed = spreadsheetsService.Query(cQuery);

                foreach (var cellAtomEntryEntry in cFeed.Entries)
                {
                    var cCell = (CellEntry) cellAtomEntryEntry;
                    Console.WriteLine("Value on row {0} and column {1} is {2}", cCell.Cell.Row,
                        cCell.Cell.Column, cCell.Cell.Value);
                }
            }


            //using (var fs = File.OpenRead(fileName))
            //{
            //    var workbook = new SpreadsheetEntry();

            //    for (var sheetIdx = 0; sheetIdx < workbook.NumberOfSheets; sheetIdx++)
            //    {

            //        ISheet sheet = workbook.GetSheetAt(sheetIdx);

            //        var datesRow = sheet.GetRow(Settings.DatesRow);

            //        if (datesRow == null)
            //            continue;

            //        var datesCell = datesRow.GetCell(Settings.DatesColumn);

            //        if (datesCell == null)
            //            continue;

            //        DateTime startDate = GetStartDate(datesCell);

            //        var sales = ParseSales(sheet, datesCell, startDate, type, producer);
            //        foreach (var s in sales)
            //            result.Add(s.Key, s.Value);
            //    }
            //}

            return result;
        }

        internal SpreadsheetFeed GetSpreadSheetsFeed()
        {
            //retrieve available spreadsheets
            var query = new SpreadsheetQuery();
            var feed = spreadsheetsService.Query(query);

            return feed;
        }

        public IEnumerable<string> GetSpreadSheetsNames()
        {
            return GetSpreadSheetsFeed().Entries.Select(e => e.Title.Text);
        }
    }
}
