using H5Api.Models;
using Microsoft.EntityFrameworkCore;

namespace H5Api.DbContexts
{
    public class StickyContext : DbContext
    {
        public StickyContext(DbContextOptions<StickyContext> options) : base(options) { }

        public DbSet<StickyData> Stickies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StickyData>().ToTable("sticky");
        }
    }
}
