using Microsoft.EntityFrameworkCore;

namespace SearchService.Data
{
    public class SearchDbContext : DbContext
    {
        public SearchDbContext(DbContextOptions<SearchDbContext> options) : base(options)
        {
        }


        public DbSet<Model.SearchProperty> SearchProperties { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.SearchProperty>()
                .HasKey(sp => sp.Id);

            //What is modelBuilder? It is a class provided by Entity Framework Core that allows you to configure the model for your database context.
            //It is used in the OnModelCreating method to define the structure of your database tables, relationships between entities, and other configurations.

            //Add area sq ft precision
            modelBuilder.Entity<Model.SearchProperty>()
                .Property(sp => sp.AreaSqFt)
                .HasColumnType("decimal(18,2)");
        }
    }
}
