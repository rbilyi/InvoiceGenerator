using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public class SaleParsed
    {
		public DateTime DateTime { get; set; }
		public string Customer { get; set; }
        public string Producer { get; set; }
		public GoodType GoodType { get; set; }
		public int Quantity { get; set; }
        public int Return { get; set; }
    }
}
