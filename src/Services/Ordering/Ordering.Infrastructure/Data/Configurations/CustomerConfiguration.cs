using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(e => e.Id).HasConversion(
											customerId => customerId.Value,
											dbId => CustomerId.Of(dbId));
			builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
			builder.Property(e => e.Email).HasMaxLength(255).IsRequired();
			builder.HasIndex(e => e.Email).IsUnique();
		}
	}
}
