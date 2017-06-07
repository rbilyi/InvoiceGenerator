using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nakladna.CommonData;
using Nakladna.Core;

namespace Nakladna
{
    public partial class MainForm : Form
    {
        List<GoodType> goodTypes;
        DbScope scope;

        public MainForm()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.ValueChanged += dateTimePicker_ValueChanged;
            dateTimePicker2.ValueChanged += dateTimePicker_ValueChanged;

            this.Text += " " + Application.ProductVersion.ToString();
        }

        private async void RefreshGrid()
        {
            ShowInToolsStrip("Loading...", true);

            await Task.Run(async () =>
            {
                if (scope != null)
                    scope.Submit();

                scope = new DbScope();
                var g = await InvoiceCore.Instance.GetGoodsAsync(scope);

                if (g == null)
                {
                    MessageBox.Show("Шото пішло не так. Типи товару не підгрузились.");
                    throw new ArgumentException("goodTypes");
                }

                goodTypes = g.ToList();
                BindGrid();
            });

            CleanToolStrip();
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

                Invoke((Action)(() => dataGridView1.DataSource = goodTypes));
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }
        }

        private async void toDocButton_Click(object sender, EventArgs e)
        {
            var startDate = dateTimePicker1.Value.Date;
            var endDate = dateTimePicker2.Value.Date;

            try
            {
                scope.Submit();
                scope = new DbScope();
                var saveDlg = new SaveFileDialog();
                saveDlg.Filter = ".docx|*.docx";
                if (saveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var runningPath = Application.StartupPath;
                    ShowInToolsStrip("Генеримо документ...", true);

                    await Task.Run(() =>
                    InvoiceCore.Instance.ExportToDoc(scope, startDate, endDate, saveDlg.FileName, runningPath));

                    ShowInToolsStrip("Документ готовий");
                    return;
                }
            }
            catch (NoSalesException)
            {
                MessageBox.Show(string.Format("Немає продаж за {0}-{1}", startDate.ToShortDateString(), endDate.ToShortDateString()));
                ShowInToolsStrip("Помилка");
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }
        }

        private async void importButton_Click(object sender, EventArgs e)
        {
            try
            {
                var sheetDialog = new SelectSheetsDialog();
                if (sheetDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (!sheetDialog.Sheets.Any())
                        return;

                    ShowInToolsStrip("Імпорутємо з файла ...", true);

                    var inv = await Task.Run(() => InvoiceCore.Instance.ImportSalesFromXLS(scope, sheetDialog.FilePath, sheetDialog.Sheets, sheetDialog.DateTime));

                    ShowInToolsStrip(string.Format("Імпортовано {0} продажів. {1} нових клієнтів.",
                        inv.Count(), InvoiceCore.Instance.NewCustomers));

                    scope.Submit();
                    scope = new DbScope();
                }
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
                ShowInToolsStrip("Помилка");
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
            toolStripProgressBar.Visible = false;
            toolStripStatusLabel.Text = message;
            toolStripStatusLabel.Visible = true;
        }

        protected void ShowInToolsStrip(string message, bool continuous)
        {
            toolStripProgressBar.Style = continuous ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
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
            if (form.ShowDialog() != DialogResult.OK)
                return;

            if (form.GoodType != null)
            {
                InvoiceCore.Instance.SaveGoodType(scope, form.GoodType);
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
                    InvoiceCore.Instance.RemoveGoodType(scope, good);
                    RefreshGrid();
                }
            }
            else if (e.ColumnIndex == ColumnSpecialPrice.Index)
            {
                var form = new SpecialPricesForm();
                form.Init(good);
                form.ShowDialog();
                RefreshGrid();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Очистити всі продажі?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                InvoiceCore.Instance.ClearSales(scope);
            }
        }

        private async void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            var date1 = dateTimePicker1.Value.Date;
            var date2 = dateTimePicker2.Value.Date;
            ShowInToolsStrip("Пошук продажів за " + date1.ToShortDateString()
                + " -- " + date2.ToShortDateString(), true);

            var salesCount = await InvoiceCore.Instance.GetSalesAsync(new DbScope(), date1, date2);

            ShowInToolsStrip("Знайдено " + salesCount.Count() + " продажів.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SalesForm(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date).Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }
}
