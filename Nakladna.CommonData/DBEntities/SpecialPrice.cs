using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
    public partial class SpecialPrice : EntityBase
    {
        public double Price { get; set; }

        public int CustomerId { get; set; }
        public int GoodTypeId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual GoodType GoodType { get; set; }
    }
}
