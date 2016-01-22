using System;
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
            context.GoodTypes.AddRange(new GoodType[] { new GoodType
                {
                    Name = "Лаваш упаковка",
                    Price = 5.8,
                    ColumnInDocument = 3,
                    ReturnColumn = 4,
                    HasReturn = true
                },
             new GoodType
            {
                Name = "Лаваш шт",
                Price = 1.5,
                ColumnInDocument = 5,
                HasReturn = false
            }, new GoodType
            {
                Name = "Гринка",
                Price = 2.1,
                ColumnInDocument = 2,
                HasReturn = false
            }
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
