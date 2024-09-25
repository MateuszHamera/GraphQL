using GraphQL.Server.Data;
using GraphQL.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Server.Queries;

public class Query
{
    public async Task<List<Author>> GetAuthors(
        [Service] AppDbContext context)
    {
        return await context.Authors.Include(x => x.Books).ToListAsync();
    }

    public async Task<List<Book>> GetBooks(
        [Service] AppDbContext context)
    {
        return await context.Books.ToListAsync();
    }
}
