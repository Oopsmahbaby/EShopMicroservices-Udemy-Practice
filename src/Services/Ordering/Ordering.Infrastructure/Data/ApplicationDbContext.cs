using Ordering.Application.Data;
using System.Reflection;

namespace Ordering.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		// Define DbSets for your entities here
		// public DbSet<Order> Orders { get; set; }

		public DbSet<Customer> Customers => Set<Customer>();
		public DbSet<Product> Products => Set<Product>();
		public DbSet<Order> Orders => Set<Order>();
		public DbSet<OrderItem> OrderItems => Set<OrderItem>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}

	}
}
