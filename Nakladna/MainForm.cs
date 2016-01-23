using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Innvoice.Generator;
using Nakladna.CommonData;
using Nakladna.Core;

namespace Nakladna
{
    public partial class MainForm : Form
    {
        private Action GoodTypesLoaded;
        List<GoodType> goodTypes;

        public MainForm()
        {
            InitializeComponent();

            this.Text += " " + Application.ProductVersion.ToString();

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            InvoiceCore.Instance.InitializationNotification += Instance_InitializationNotification;
            GoodTypesLoaded = () => BindGrid();


            RefreshGrid();
        }

        private async void RefreshGrid()
        {
            var g = await InvoiceCore.Instance.GetGoodsAsync();

            if (g == null)
            {
                MessageBox.Show("Шото пішло не так. Типи товару не підгрузились.");
                throw new ArgumentException("goodTypes");
            }

            goodTypes = g.ToList();
            GoodTypesLoaded();
        }

        private void BindGrid()
        {
            try
            {
                if (goodTypes == null)
                    return;

                dataGridView1.AutoGenerateColumns = false;
                ColumnGoodType.DataPropertyName = "Name";
                ColumnGoodPrice.DataPropertyName = "Price";
                ColumnColumn.DataPropertyName = "ColumnName";
                ColumnReturn.DataPropertyName = "ReturnColumnName";
                ColumnHasReturn.DataPropertyName = "HasReturn";

                dataGridView1.DataSource = goodTypes;
                CleanToolStrip();
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }
        }

        void Instance_InitializationNotification(object sender, NotificationEventArgs e)
        {
            ShowInToolsStrip(e.Message);
        }

        private async void toDocButton_Click(object sender, EventArgs e)
        {
            var startDate = dateTimePicker1.Value;
            var endDate = dateTimePicker2.Value;

            try
            {
                var saveDlg = new SaveFileDialog();
                saveDlg.Filter = ".docx|*.docx";
                if (saveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var runningPath = Application.StartupPath;
                    statusProgress("Генеримо документ...", true);

                    await Task.Run(() =>
                    InvoiceCore.Instance.ExportToDoc(startDate, endDate, saveDlg.FileName, runningPath));

                    statusProgress("Документ готовий", false);
                    return;
                }
            }
            catch (NoSalesException)
            {
                MessageBox.Show(string.Format("Немає продаж за {0}-{1}", startDate.ToShortDateString(), endDate.ToShortDateString()));
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }

            statusProgress("", false);
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            try
            {
                var sheetDialog = new SelectSheetsDialog();
                if (sheetDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {

                    var inv = InvoiceCore.Instance.ImportSalesFromXLS(sheetDialog.FilePath, sheetDialog.Sheets, sheetDialog.DateTime);
                    MessageBox.Show(string.Format("Імпортовано {0} продажів. {1} нових клієнтів.",
                        inv.Count(), InvoiceCore.Instance.NewCustomers));
                }
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }
        }

        private void ShowErrorBox(Exception ex)
        {
            MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Помилка");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var set = new SettingsWindow();
            set.ShowDialog(this);
        }

        protected void ShowInToolsStrip(string message)
        {
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Step = 1;
            toolStripStatusLabel.Text = message;
            toolStripStatusLabel.Visible = true;
        }

        protected void CleanToolStrip()
        {
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Visible = false;
        }

        private void btnAddGood_Click(object sender, EventArgs e)
        {
            var form = new GoodTypeForm();
            form.ShowDialog();

            if (form.GoodType != null)
            {
                InvoiceCore.Instance.SaveGoodType(form.GoodType);
                RefreshGrid();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            GoodType good;
            try
            {
                good = dataGridView1.Rows[e.RowIndex].DataBoundItem as GoodType;
                if (good == null)
                    return;
            }
            catch
            {
                return;
            }

            if (e.ColumnIndex == ColumnRemove.Index)
            {
                if (MessageBox.Show("Видалити " + good.Name + "?", "Видалення", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    InvoiceCore.Instance.RemoveGoodType(good);
                    BindGrid();
                }
            }
            else if (e.ColumnIndex == ColumnSpecialPrice.Index)
            {
                var form = new SpecialPricesForm();
                form.Show();
                form.Init(good);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            InvoiceCore.Instance.SaveDataChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистити всі продажі?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                InvoiceCore.Instance.ClearSales();
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            //var date1 = dateTimePicker1.Value;
            //var date2 = dateTimePicker2.Value;
            //statusProgress("Пошук продажів за " + date1.ToShortDateString()
            //    + " -- " + date2.ToShortDateString(), true);


            //var salesCount = await Task.Run(() =>
            //InvoiceCore.Instance.GetSales(date1, date2));

            //statusProgress("Знайдено " + salesCount.Count() + " продажів.", false);
        }

        private void statusProgress(string text, bool showProgress)
        {
            toolStripStatusLabel.Text = text;
            toolStripStatusLabel.Visible = !(string.IsNullOrWhiteSpace(text));
            toolStripProgressBar.Visible = showProgress;
        }
    }
}
