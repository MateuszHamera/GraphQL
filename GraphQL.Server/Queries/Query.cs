using GraphQL.Server.Data;
using GraphQL.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Server.Queries;

public class Query
{
    public async Task<List<Author>> GetAuthors(
        [Service] AppDbContext context)
    {
        return await context.Authors.ToListAsync();
    }
}
