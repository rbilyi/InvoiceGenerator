using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nakladna
{
    public partial class SelectSheetsDialog : Form
    {
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            private set
            {
                filePath = value;
                txtFilePath.Text = value;
            }
        }

        public IEnumerable<int> Sheets { get; private set; }

        public DateTime DateTime { get; private set; }

        public SelectSheetsDialog()
        {
            InitializeComponent();

            dateTimePicker1.Value = DateTime.Now;
        }

        public SelectSheetsDialog(string filePath)
            : this()
        {
            if (string.IsNullOrEmpty(filePath))
            {
                BrowseFile();
            }
            else
            {
                FilePath = filePath;
            }

            FillShets();
        }

        private void FillShets()
        {
            var sheets = Core.InvoiceCore.Instance.GetSheets(FilePath);
            foreach (var sheet in sheets.OrderByDescending(s => s.Key))
            {
                listBox1.Items.Add(new SheetItemInListBox(sheet.Key, sheet.Value));
            }
        }

        private void BrowseFile()
        {
            using (var openDialog = new OpenFileDialog())
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    FilePath = openDialog.FileName;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Sheets = listBox1.SelectedItems.Cast<SheetItemInListBox>().Select(i => i.Index);
            DateTime = dateTimePicker1.Value;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            BrowseFile();
        }

        internal class SheetItemInListBox
        {
            private string text;

            public int Index { get; private set; }
            public SheetItemInListBox(int index, string text)
            {
                Index = index;
            }

            public override string ToString()
            {
                return Index + " " + text;
            }
        }
    }
}
