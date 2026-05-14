using Microsoft.EntityFrameworkCore;

namespace SearchService.Data
{
    public class SearchDbContext : DbContext
    {
        public SearchDbContext(DbContextOptions<SearchDbContext> options) : base(options)
        {
        }


        public DbSet<Model.SearchProperty> SearchProperties { get; set; }
    }
}
