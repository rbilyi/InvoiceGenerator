using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nakladna
{
    public partial class SelectSheetsDialog : Form
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            private set
            {
                _filePath = value;
                txtFilePath.Text = value;
                Settings.ExcellFilePath = value;
                Settings.Save();
            }
        }

        public IEnumerable<int> Sheets { get; private set; }

        public DateTime DateTime { get; private set; }

        public SelectSheetsDialog()
        {
            InitializeComponent();

            dateTimePicker1.Value = DateTime.Now.Date;
            FilePath = Settings.ExcellFilePath;

            if (string.IsNullOrEmpty(FilePath))
                FilePath = BrowseFile();

            FillShets();
        }

        private void FillShets()
        {
            if (string.IsNullOrEmpty(FilePath))
                return;

            listBox1.Items.Clear();

            try
            {
                var sheets = Core.InvoiceCore.Instance.GetSheets(FilePath);
                foreach (var sheet in sheets.OrderByDescending(s => s.Key))
                {
                    listBox1.Items.Add(new SheetItemInListBox(sheet.Key, sheet.Value));
                }
            }
            catch (IOException)
            {
                return;
            }
        }

        private string BrowseFile()
        {
            using (var openDialog = new OpenFileDialog())
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    return openDialog.FileName;
                }
            }

            return null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Sheets = listBox1.SelectedItems.Cast<SheetItemInListBox>().Select(i => i.Index);
            DateTime = dateTimePicker1.Value;
            Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var path = BrowseFile();
            if (string.IsNullOrEmpty(path))
                return;

            FilePath = path;
            FillShets();
        }

        internal class SheetItemInListBox
        {
            private string text;

            public int Index { get; private set; }
            public SheetItemInListBox(int index, string text)
            {
                Index = index;
                this.text = text;
            }

            public override string ToString()
            {
                return Index + " " + text;
            }
        }
    }
}
