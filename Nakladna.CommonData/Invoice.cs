using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
	public class Invoice
	{
		public Customer Customer { get; set; }
		public DateTime DateTime { get; set; }

        public string Producer { get; set; }

        /// <summary>
        /// Gets or sets the supplies.
        /// Key represents the good type.
        /// Value perresents the quontity.
        /// </summary>
        /// <value>
        /// The purhases.
        /// </value>
        public List<SoldItem> SoldItems { get; set; }

		public double TotalPrice
		{
			get
			{
				return SoldItems.Sum(s => s.Price * s.Qty);
			}
		}

		public int TotalCount
		{
			get
			{
				return SoldItems.Sum(s => s.Qty);
			}
		}
    }
}
