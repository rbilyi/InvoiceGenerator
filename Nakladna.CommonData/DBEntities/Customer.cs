using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
	public partial class Customer: EntityBase
	{
		public string Name { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }

		public virtual ICollection<Sale> Sales { get; set; }
        public virtual ICollection<SpecialPrice> SepicalPrices { get; set; }
	}
}
