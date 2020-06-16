using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess.Entities;

namespace WebApplication1.DataAccess.Contexts
{
    public sealed class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTranslation> PostTranslations { get; set; }
        public DbSet<PostMeta> PostMetas { get; set; }
    }
}
