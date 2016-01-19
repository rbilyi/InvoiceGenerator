using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nakladna.DAL.Entitties
{
	public static class EnumsConverter
	{
		public static string GetString(GoodTypes type)
		{
			switch (type)
			{
				case GoodTypes.Lavash:
					return "Лаваш";
				default:
					throw new ArgumentNullException("type");
			}
		}
	}

	public enum GoodTypes
	{
		Lavash
	}
}
