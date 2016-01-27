using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.CommonData
{
	public abstract partial class EntityBase
	{
		public int? Id { get; set; }
		public bool IsDeleted { get; set; }
	}
}
