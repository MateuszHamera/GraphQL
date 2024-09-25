using GraphQL.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Server.Data;

public class AppDbContext : DbContext 
{
    public AppDbContext(
    DbContextOptions<AppDbContext> options)
    : base(options)
    {
        
    }

    public DbSet<Author> Authors { get; set; } = default!;

    public DbSet<Book> Books { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);
    }
}
