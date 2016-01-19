using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nakladna.CommonData;

namespace Nakladna
{
    public partial class GoodTypeForm : Form
    {
        public GoodType GoodType { get; private set; }

        public GoodTypeForm()
        {
            InitializeComponent();
        }

        public GoodTypeForm(GoodType goodType)
            :this()
        {
            fillGood(goodType);
        }

        private void fillGood(GoodType goodType)
        {
            txtColumn.Text = goodType.ColumnInDocument.ToString();
            txtPrice.Text = goodType.Price.ToString();
            txtTitle.Text = goodType.Name;

            if (goodType.HasReturn)
            {
                checkBox1.Checked = true;
                txtReturnColumn.Text = goodType.ReturnColumnt.ToString();
            }
            else
            {
                checkBox1.Checked = false;
            }
        }

        private bool buildGood()
        {
            try
            {
                var goodType = new GoodType();
                txtPrice.Text = txtPrice.Text.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                txtPrice.Text = txtPrice.Text.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                goodType.Name = txtTitle.Text;
                goodType.Price = double.Parse(txtPrice.Text.Trim());
                goodType.ColumnInDocument = int.Parse(txtColumn.Text.Trim());
                goodType.HasReturn = checkBox1.Checked;
                goodType.ReturnColumnt = int.Parse(txtReturnColumn.Text.Trim());
                GoodType = goodType;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtReturnColumn.Enabled = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (buildGood())
                Close();
        }
    }
}
