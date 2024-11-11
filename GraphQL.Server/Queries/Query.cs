using GraphQL.Server.Data;
using GraphQL.Server.Models;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Server.Queries;

public class Query
{
    [UseSorting]
    [UseFiltering]
    [Authorize]
    public IQueryable<Author> GetAuthors(
        [Service] AppDbContext context)
    {
        return context.Authors
            .Include(x => x.Books);
    }

    [Authorize(Policy = "AdminPolicy")]
    public async Task<Author> GetAuthorById(
        int id,
        [Service] AppDbContext context)
    {
        return await context.Authors.Include(x => x.Books)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    [Authorize(Policy = "UserPolicy")]
    public IQueryable<Book> GetBooks(
        [Service] AppDbContext context)
    {
        return context.Books;
    }
}
