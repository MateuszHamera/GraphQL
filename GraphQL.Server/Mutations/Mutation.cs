using GraphQL.Server.Data;
using GraphQL.Server.Inputs;
using GraphQL.Server.Models;
using GraphQL.Server.Payloads;

namespace GraphQL.Server.Mutations;

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

    public async Task<AddBookPayload> AddBookAsync(
        AddBookInput input,
        [Service] AppDbContext context)
    {
        var author = await context.Authors.FindAsync(input.authorId);

        if (author == null)
        {
            throw new Exception($"Author with Id {input.authorId} not found.");
        }

        var book = new Book()
        {
            Title = input.title,
            Description = input.description,
            Price = input.price,
            Author = author 
        };

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        return new AddBookPayload(book);
    }

}
