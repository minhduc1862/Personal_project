using Microsoft.EntityFrameworkCore;
using person_projectAPI.Models;
using personal_projectAPI.Models;

namespace person_projectAPI.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }
        #region DbSet
        public DbSet<ItemNews> News { get; set; }
        public DbSet<ItemSlide> Slides { get; set; }
        public DbSet<ItemProduct> Products { get; set; }
        public DbSet<ItemRating> Rating { get; set; }
        #endregion
    }
}
