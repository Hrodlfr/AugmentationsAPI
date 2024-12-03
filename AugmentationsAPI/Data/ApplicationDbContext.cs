namespace AugmentationsAPI.Data
{
    using Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using AugmentationsAPI.Data.Seeding;

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Augmentation> Augmentations => Set<Augmentation>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Augmentations into the Database
            new SeederAugmentation().Configure(builder.Entity<Augmentation>());
        }

    }
}