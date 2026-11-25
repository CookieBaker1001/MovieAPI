using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<Movie> Movie { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<Actor> Actors { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .ToTable("Movies")
            .HasKey(m => m.Id);
        modelBuilder.Entity<Movie>()
            .Property(m => m.Title)
            .IsRequired();
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Details)
            .WithOne(md => md.Movie)
            .HasForeignKey<MovieDetails>(md => md.MovieId);
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Reviews)
            .WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId)
            .IsRequired();
        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Actors)
            .WithMany(a => a.Movies);

        modelBuilder.Entity<MovieDetails>()
            .ToTable("MovieDetails")
            .HasKey(md => md.Id);

        modelBuilder.Entity<Actor>()
            .ToTable("Actors")
            .HasKey(a => a.Id);
        modelBuilder.Entity<Actor>()
            .Property(a => a.Name)
            .IsRequired();

        modelBuilder.Entity<Review>()
            .ToTable("Reviews")
            .HasKey(r => r.Id);
        modelBuilder.Entity<Review>()
            .Property(r => r.ReviewerName)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
