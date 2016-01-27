using System.Data.Entity;
using Nakladna.CommonData;

namespace Nakladna.DAL
{
	public class InvoicesContext: DbContext
	{
        public InvoicesContext()
            :base()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InvoicesContext,
                Migrations.Configuration>());
        }

		public DbSet<Sale> Sales{ get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<GoodType> GoodTypes { get; set; }
		public DbSet<SpecialPrice> SpecialPrices { get; set; }
    } 
}
