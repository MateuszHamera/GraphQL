using GraphQL.Server.Models;

namespace GraphQL.Server.Payloads;

public class AddBookPayload
{
    public AddBookPayload(Book book)
    {
        Book = book;
    }

    public Book Book { get; set; }
}
