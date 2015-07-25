using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.DAL.Entitties
{
	public static class EnumsConverter
	{
		public static string GetString(GoodType type)
		{
			switch (type)
			{
				case GoodType.Lavash:
					return "Лаваш";
				default:
					throw new ArgumentNullException("type");
			}
		}
	}

	public enum GoodType
	{
		Lavash
	}
}
