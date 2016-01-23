using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;
using Novacode;

namespace Innvoice.Generator
{
    public class InvoiceGenerator
    {
        private void Create()
        {
            string fileName = "doc.docx";
            DocX doc = DocX.Create(fileName, DocumentTypes.Document);

            doc.InsertParagraph("This is my first paragraph");
            doc.Save();

            Process.Start(fileName);
        }

        public void CreateInOneDocument(IEnumerable<Invoice> invoices, string docSavePath, string runningPath)
        {
            string fileName = docSavePath;
            var tempPath = CopyToTemp(Path.Combine(runningPath, Nakladna.Settings.TemplateFileName));
            int invNum = 1;
            DocX resultDoc = null;

            foreach (var invoice in invoices)
            {
                using (var t = DocX.Load(tempPath))
                {
                    t.ReplaceText("#invoiceNumber", invNum.ToString());
                    t.ReplaceText("#dayN", invoice.DateTime.Day.ToString());
                    t.ReplaceText("#monthN", invoice.DateTime.Month.ToString());
                    t.ReplaceText("#yearN", invoice.DateTime.Year.ToString());
                    t.ReplaceText("#whoTo", invoice.Customer.Name);
                    t.ReplaceText("#From", invoice.Producer);

                    var itemsTable = t.Tables[1];

                    int row = 1;
                    foreach (var s in invoice.SoldItems)
                    {
                        if (row <= itemsTable.RowCount)
                        {
                            var c = itemsTable.Rows[row].Cells;
                            c[0].Paragraphs.First().InsertText(row.ToString());
                            c[1].Paragraphs.First().InsertText(s.GoodType.Name);
                            c[3].Paragraphs.First().InsertText(s.Qty.ToString());
                            c[4].Paragraphs.First().InsertText(string.Format("{0:f2}", s.Price));
                            c[5].Paragraphs.First().InsertText(string.Format("{0:f2}", s.Price * s.Qty));
                            row++;
                        }
                    }

                    t.ReplaceText("#totPrc", string.Format("{0:f2}", invoice.TotalPrice));
                    t.ReplaceText("#totalCount", string.Format("{0:f2}", invoice.TotalPrice));
                    t.ReplaceText("#totalPriceD", ((int)invoice.TotalPrice).ToString());
                    var kop = Math.Ceiling((invoice.TotalPrice - (int)invoice.TotalPrice) * 100).ToString();
                    t.ReplaceText("#totalPriceF", kop);

                    if (resultDoc == null)
                    {
                        t.SaveAs(fileName);
                        resultDoc = DocX.Load(fileName);
                    }
                    else
                    {
                        resultDoc.InsertDocument(t);
                    }
                }

                invNum++;
            }

            if (resultDoc != null)
            {
                resultDoc.Save();
                resultDoc.Dispose();

                Process.Start(fileName);
            }
        }

        private static string CopyToTemp(string originalFile)
        {
            var tempFileName = Path.GetTempFileName() + Path.GetExtension(originalFile);
            File.Copy(originalFile, tempFileName, true);
            return tempFileName;
        }
    }
}
