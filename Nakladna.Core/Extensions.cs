using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;

namespace Nakladna.Core
{
    public static class Extensions
    {
        public static Sale ToSale(this SaleParsed s)
        {
            var customer =  InvoiceCore.Instance.GetCustomer(c => c.Name == s.Customer);
            if (customer == null)
                customer = new Customer()
                {
                    Name = s.Customer
                };


            return new Sale()
            {
                Customer = customer,
                Producer = s.Producer,
                DateTime = s.DateTime,
                GoodType = s.GoodType,
                Quantity = s.Quantity
            };
        }
    }
}
