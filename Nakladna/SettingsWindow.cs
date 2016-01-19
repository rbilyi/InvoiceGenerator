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
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();

            try
            {
                connectionStringTxt.Text = Settings.ConnectionString;
                custColTxt.Text = Settings.CustomersColumn.ToString();
                custRowTxt.Text = Settings.CustomersRow.ToString();
                producerTxt.Text = Settings.Producer;
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
                Settings.CustomersColumn = int.Parse(custColTxt.Text);
                Settings.CustomersRow = int.Parse(custRowTxt.Text);
                Settings.Producer = producerTxt.Text;
            }
            catch
            {
                MessageBox.Show("Error while loading settings");
            }
            Settings.Save();
            this.Close();
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
