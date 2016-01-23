using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public class SoldItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoldItem"/> class.
        /// </summary>
        /// <param name="goodType">Type of the good.</param>
        /// <param name="qty">The qty.</param>
        /// <param name="price">The custom price.</param>
        public SoldItem(GoodType goodType, int qty, double? price = null)
        {
            GoodType = goodType;
            Qty = qty;
            Price = price ?? goodType.Price;
        }
        public GoodType GoodType { get; protected set; }
        public int Qty { get; protected set; }

        public double Price { get; protected set; }
    }
}
