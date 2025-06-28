using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Data
{
	public class CatalogDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }

		public CatalogDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
				.HasKey(b => b.Id);

			modelBuilder.Entity<Product>()
				.HasOne<Category>()
				.WithMany()
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Category>()
				.HasKey(b => b.Id);

			modelBuilder.Entity<Category>()
				.HasOne<Category>()
				.WithMany()
				.IsRequired(false)
				.HasForeignKey(_ => _.ParentId);

			modelBuilder.Entity<Product>()
				.OwnsOne(p => p.Price);


		}
	}
}
