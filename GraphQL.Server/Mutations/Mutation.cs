using GraphQL.Server.Data;
using GraphQL.Server.Inputs;
using GraphQL.Server.Models;
using GraphQL.Server.Payloads;

namespace GraphQL.Server.Mutations
{
    public class Mutation
    {
        public async Task<AddAuthorPayload> AddAuthorAsync(
            AddAuthorInput input,
            [Service] AppDbContext context)
        {
            var author = new Author()
            {
                Name = input.name,
                Age = input.age
            };

            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();

            return new AddAuthorPayload(author);
        }
    }
}
