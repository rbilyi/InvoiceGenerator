using System;
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
        {
            InitializeComponent();

            GoodType = goodType;
            fillGood(goodType);
        }

        private void fillGood(GoodType goodType)
        {
            txtColumn.Text = goodType.ColumnName;
            txtPrice.Text = goodType.Price.ToString();
            txtTitle.Text = goodType.Name;

            if (goodType.HasReturn)
            {
                checkBox1.Checked = true;
                txtReturnColumn.Text = goodType.ReturnColumnName;
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
                if (GoodType == null)
                    GoodType = new GoodType();

                txtPrice.Text = txtPrice.Text.Replace(",", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                txtPrice.Text = txtPrice.Text.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
                GoodType.Name = txtTitle.Text;
                GoodType.Price = double.Parse(txtPrice.Text.Trim());
                GoodType.ColumnName = txtColumn.Text.Trim();
                GoodType.ReturnColumnName = txtReturnColumn.Text.Trim();
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
                DialogResult = DialogResult.OK;
        }
    }
}
