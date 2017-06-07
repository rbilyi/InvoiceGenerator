using System;
using System.Windows.Forms;

namespace Nakladna
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();

            try
            {
                connectionStringTxt.Text = Settings.ConnectionString;
                custColTxt.Text = Settings.CustomersColumn.ToString();
                customerStopPhraseTxt.Text = Settings.CustomerStopPhrase;
                custRowTxt.Text = Settings.CustomersRow.ToString();
                producerTxt.Text = Settings.Producer;
                templtePathTxt.Text = Settings.TemplateFileName;
            }
            catch
            {
                MessageBox.Show("Error while loading settings");
            }
        }

        private void saveBnt_Click(object sender, EventArgs e)
        {
            try
            {
                Settings.ConnectionString = connectionStringTxt.Text;
                Settings.CustomerStopPhrase = customerStopPhraseTxt.Text;
                Settings.CustomersColumn = int.Parse(custColTxt.Text);
                Settings.CustomersRow = int.Parse(custRowTxt.Text);
                Settings.TemplateFileName = templtePathTxt.Text;
                Settings.Producer = producerTxt.Text;

                Settings.Save();
                DialogResult = DialogResult.OK;
            }
            catch
            {
                MessageBox.Show("Error while loading settings");
            }
        }

        private void reloadBtn_Click(object sender, EventArgs e)
        {
            Settings.Reload();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            Settings.Reset();
        }
    }
}
