using Microsoft.EntityFrameworkCore;
using Movie_Api.Entities;

namespace Movie_Api.DbContexts
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntitySettings(modelBuilder);
        }

        private void EntitySettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                 .ToTable("movie")
                 .HasKey(x => x.Id)
                 .HasName("movieId");

            modelBuilder.Entity<Movie>()
                .HasIndex(x => x.ImdbId)
                .IsUnique();

            modelBuilder.Entity<Review>()
                .ToTable("review")
                .HasKey(x => x.Id)
                .HasName("reviewId");

            modelBuilder.Entity<Review>()
                .HasOne(x => x.Movie)
                .WithMany()
                .HasForeignKey(x => x.MovieId);
        }
    }
}
