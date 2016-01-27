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
using Nakladna.Core;

namespace Nakladna
{
    public partial class SpecialPricesForm : Form
    {
        IEnumerable<GoodType> goods;
        IEnumerable<SpecialPrice> specialPrices;
        DbScope scope;

        public SpecialPricesForm()
        {
            InitializeComponent();
        }

        public async void Init(GoodType goodType)
        {
            scope = new DbScope();
            goods = await InvoiceCore.Instance.GetGoodsAsync(scope);
            cmbGoodType.DataSource = goods.Select(g => g.Name).ToList();
            cmbGoodType.SelectedItem = goodType.Name;

            await RebindGrid(goodType);

            cmbGoodType.SelectedIndexChanged += CmbGoodType_SelectedIndexChanged;
        }

        private async Task RebindGrid(GoodType goodType)
        {
            specialPrices = await InvoiceCore.Instance.GetSpecialPricesAsync(scope);
            var source = specialPrices.ToList().Where(p => p.GoodTypeId == goodType.Id);

            dataGridView1.AutoGenerateColumns = false;
            ColumnPrice.DataPropertyName = "Price";
            СolumnClinetName.DataPropertyName = "CustomerName";
            dataGridView1.DataSource = source.ToList();

            var clients = InvoiceCore.Instance.GetCustomers(scope);
            cmbClient.DataSource = clients
                .Where(c => !source.Any(sp => sp.Customer.Name == c.Name))
                .Select(c => c.Name).ToList();
        }

        private async void CmbGoodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var good = goods.First(g => g.Name == (string)((ComboBox)sender).SelectedItem);
            await RebindGrid(good);
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var good = goods.First(g => g.Name == cmbGoodType.SelectedItem.ToString());
            var client = InvoiceCore.Instance.GetCustomers(scope).First(c => c.Name == cmbClient.SelectedItem.ToString());
            InvoiceCore.Instance.AddSpecialPrice(scope, good, client, good.Price);
            await RebindGrid(good);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
