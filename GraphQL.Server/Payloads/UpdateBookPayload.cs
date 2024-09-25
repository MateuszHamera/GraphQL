using GraphQL.Server.Models;

namespace GraphQL.Server.Payloads;

public class UpdateBookPayload
{
    public UpdateBookPayload(Book book)
    {
        Book = book;
    }
    public Book Book { get; set; }
}
