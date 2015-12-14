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
    public partial class Form1 : Form
    {
        GoodType[] goodTypes;

        public Form1()
        {
            InitializeComponent();

            this.Text += " " + Application.ProductVersion.ToString();

            try
            {
                InvoiceCore.Instance.InitializationNotification += Instance_InitializationNotification;

                goodTypes = InvoiceCore.Instance.GetGoods().ToArray();

                if (goodTypes == null || !goodTypes.Any())
                {
                    MessageBox.Show("Шото пішло не так. Типи товару не підгрузились.");
                    throw new ArgumentException("goodTypes");
                }
                comboBoxGoodType.DataSource = goodTypes;
                comboBoxGoodType.SelectedItem = comboBoxGoodType.Items[0];

                producerName.Text = Settings.Producer;

                if (Settings.LastImportedDate.HasValue)
                    dateTimePicker1.Value = Settings.LastImportedDate.Value;

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

        private void toDocButton_Click(object sender, EventArgs e)
        {
            var startDate = dateTimePicker1.Value;
            var endDate = dateTimePicker2.Value;

            try
            {
                var producer = producerName.Text;

                if (string.IsNullOrEmpty(producer))
                    throw new ArgumentException("Введіть поставщика!");

                Settings.Producer = producer;
                Settings.Save();

                var saveDlg = new SaveFileDialog();
                saveDlg.Filter = ".docx|*.docx";
                if (saveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var runningPath = Application.StartupPath;
                    InvoiceCore.Instance.ExportToDoc(startDate, endDate, saveDlg.FileName, runningPath);
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
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            try
            {
                var openDlg = new OpenFileDialog();
                if (openDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(openDlg.FileName))
                        return;

                    var producer = producerName.Text;
                    if (string.IsNullOrEmpty(producer))
                        throw new ArgumentException("Введіть поставщика!");

                    Settings.Producer = producer;
                    Settings.Save();

                    var inv = InvoiceCore.Instance.ImportSalesFromXLS(openDlg.FileName, comboBoxGoodType.SelectedValue as GoodType, producer);
                    MessageBox.Show(string.Format("Імпортовано {0} продажів. {1} нових клієнтів.",
                        inv.Count(), InvoiceCore.Instance.NewCustomers));
                }
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }
        }

        private void savePriceButton_Click(object sender, EventArgs e)
        {
            try
            {
                var producer = producerName.Text;
                if (string.IsNullOrEmpty(producer))
                    throw new ArgumentException("Введіть поставщика!");

                Settings.Producer = producer;
                Settings.Save();

                var goodType = comboBoxGoodType.SelectedValue as GoodType;
                priceTextBox.Text = priceTextBox.Text.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                priceTextBox.Text = priceTextBox.Text.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                goodType.Price = double.Parse(priceTextBox.Text);
                InvoiceCore.Instance.SaveEntitiesChanges();
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

        private void comboBoxGoodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                priceTextBox.Text = string.Format("{0:f2}", (comboBoxGoodType.SelectedValue as GoodType).Price);
            }
            catch (Exception ex)
            {
                ShowErrorBox(ex);
            }
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
    }
}
