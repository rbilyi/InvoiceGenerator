using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public class Sale : EntityBase
    {
        public DateTime DateTime { get; set; }
        public int Quantity { get; set; }
        public int Return { get; set; }

        public string Producer { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual GoodType GoodType { get; set; }
    }
}
