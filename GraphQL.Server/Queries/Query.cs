using GraphQL.Server.Data;
using GraphQL.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Server.Queries;

public class Query
{
    [UseSorting]
    [UseFiltering]
    public IQueryable<Author> GetAuthors(
        [Service] AppDbContext context)
    {
        return context.Authors
            .Include(x => x.Books);
    }

    public async Task<Author> GetAuthorById(
        int id,
        [Service] AppDbContext context)
    {
        return await context.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Book>> GetBooks(
        [Service] AppDbContext context)
    {
        return await context.Books.ToListAsync();
    }
}
