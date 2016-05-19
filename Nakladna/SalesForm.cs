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
    public partial class SalesForm : Form
    {
        public SalesForm()
        {
            InitializeComponent();

            LoadSales();
        }

        private async Task LoadSales()
        {
            using (var scope = new Core.DbScope())
            {
                dataGridView1.DataSource = await Core.InvoiceCore.Instance.GetSalesAsync(scope, DateTime.Today);
            }
        }
    }
}
