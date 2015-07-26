using System.Data.Entity;
using Nakladna.CommonData;

namespace Nakladna.DAL
{
	public class InvoicesContext: DbContext
	{
		public InvoicesContext(string connectionString)
			: base(connectionString)
		{
            Database.SetInitializer<InvoicesContext>(new InvoicesInitializer());
		}
		public DbSet<Sale> Sales{ get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<GoodType> GoodTypes { get; set; }
	} 
}
