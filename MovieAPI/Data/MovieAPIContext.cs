using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<Movie> Movie { get; set; } = default!;
}
