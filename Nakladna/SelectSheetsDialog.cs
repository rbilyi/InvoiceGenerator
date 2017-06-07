using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nakladna.CommonData;

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

        private async void FillShets()
        {
            if (string.IsNullOrEmpty(FilePath))
                return;

            await Task.Run((Action)AddItems);
            progressBar1.Visible = false;
            listBox1.Visible = true;
        }

        private void AddItems()
        {
            try
            {
                var sheets = Core.InvoiceCore.Instance.GetSheets(FilePath);
                listBox1.DataSource = sheets.OrderBy(s => s.Index).ToList();
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
            Sheets = listBox1.SelectedItems.Cast<SheetItem>().Select(i => i.Index);
            DateTime = dateTimePicker1.Value;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var path = BrowseFile();
            if (string.IsNullOrEmpty(path))
                return;

            FilePath = path;
            FillShets();
        }
    }
}
