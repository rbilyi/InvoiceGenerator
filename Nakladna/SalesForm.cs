using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nakladna
{
    public partial class SalesForm : Form
    {
        public SalesForm()
        {
            InitializeComponent();
        }

        public SalesForm(DateTime dateFrom, DateTime dateTo)
            : this()
        {
            dateTimePickerFrom.Value = dateFrom;
            dateTimePickerTo.Value = dateTo;

            if (dateFrom == dateTo)
                chkTo.Checked = false;
        }

        private async Task LoadSales()
        {
            dataGridView1.Visible = !(progressBar1.Visible = true);

            using (var scope = new Core.DbScope())
            {
                var dateTo = chkTo.Checked ? dateTimePickerTo.Value.Date : dateTimePickerFrom.Value.Date;
                dataGridView1.DataSource = await Core.InvoiceCore.Instance.GetSalesAsync(scope, dateTimePickerFrom.Value.Date, dateTo);
            }

            dataGridView1.Visible = !(progressBar1.Visible = false);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerTo.Enabled = chkTo.Checked;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            lblSelectDates.Visible = false;
            Task t = LoadSales();
        }
    }
}
