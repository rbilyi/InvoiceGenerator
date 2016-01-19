using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
	public class GoodType: EntityBase
	{
		public string Name { get; set; }
		public double Price { get; set; }
        public int ColumnInDocument { get; set; }
        public bool HasReturn { get; set; }

        public int? ReturnColumnt { get; set; }

        public virtual IEnumerable<Sale> Sales { get; set; }

        public override string ToString()
        {
            return Name;
        }
	}
}
