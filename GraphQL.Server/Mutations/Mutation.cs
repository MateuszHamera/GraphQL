using GraphQL.Server.Data;
using GraphQL.Server.Inputs;
using GraphQL.Server.Models;
using GraphQL.Server.Payloads;
using GraphQL.Server.Services;
using GraphQL.Server.Subscriptions;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Server.Mutations;

public class Mutation
{
    private readonly AuthService _authService;

    public Mutation(AuthService authService)
    {
        _authService = authService;
    }

    public LoginPayload LoginToServer(string email, string password)
    {
        if (email == "admin@admin" && password == "admin")
        {
            var token = _authService.GenerateJwtToken(email, "admin");
            return new LoginPayload(token);
        }
        else
        {
            var token = _authService.GenerateJwtToken(email, "user");
            return new LoginPayload(token);
        }
    }

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

    public async Task<UpdateBookPayload> UpdateBookPriceAsync(
        int bookId, double newPrice,
        [Service] AppDbContext context,
        [Service] ITopicEventSender eventSender)
    {
        var book = await context.Books
            .Include(b => b.Author)  
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null)
        {
            throw new Exception($"Book with Id {bookId} not found.");
        }

        book.Price = newPrice;
        await context.SaveChangesAsync();

        await eventSender.SendAsync(nameof(Subscription.OnBookPriceUpdated), book);

        return new UpdateBookPayload(book);
    }

}
