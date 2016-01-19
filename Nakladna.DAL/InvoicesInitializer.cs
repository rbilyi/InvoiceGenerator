﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nakladna.CommonData;

namespace Nakladna.DAL
{
	public class InvoicesInitializer : DropCreateDatabaseIfModelChanges<InvoicesContext>
	{
		protected override void Seed(InvoicesContext context)
		{
			var lavash = new GoodType
				{
					Name = "Лаваш",
					Price = 5.8,
                    ColumnInDocument = 1,
                    ReturnColumnt = 2
				};

			context.GoodTypes.Add(lavash);
			context.SaveChanges();

			base.Seed(context);
		}
	}
}
