using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public partial class SpecialPrice : EntityBase
    {
        public Customer Customer { get; set; }
        public GoodType GoodType { get; set; }
        public double Price { get; set; }
    }
}
