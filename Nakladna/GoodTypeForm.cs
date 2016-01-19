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
        {
            InitializeComponent();

            GoodType = goodType;
            fillGood(goodType);
        }

        private void fillGood(GoodType goodType)
        {
            txtColumn.Text = GetExcelColumnName(goodType.ColumnInDocument);
            txtPrice.Text = goodType.Price.ToString();
            txtTitle.Text = goodType.Name;

            if (goodType.HasReturn)
            {
                checkBox1.Checked = true;
                txtReturnColumn.Text = GetExcelColumnName(goodType.ReturnColumn.Value);
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
                GoodType.ColumnInDocument = GetColumnNumber(txtColumn.Text.Trim());
                GoodType.HasReturn = checkBox1.Checked;
                GoodType.ReturnColumn = GetColumnNumber(txtReturnColumn.Text.Trim());
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

        public static int GetColumnNumber(string name)
        {
            int number = 0;
            int pow = 1;
            for (int i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }

            return number;
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
    }
}
