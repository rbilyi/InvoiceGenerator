using System.Collections.Generic;

namespace Nakladna.CommonData
{
    public partial class GoodType : EntityBase
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int ColumnInDocument { get; set; }

        public int? ReturnColumn { get; set; }

        public virtual IEnumerable<Sale> Sales { get; set; }

        public virtual IEnumerable<SpecialPrice> SpecialPrices { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
