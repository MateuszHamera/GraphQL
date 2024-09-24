using GraphQL.Server.Models;

namespace GraphQL.Server.Payloads;

public class AddAuthorPayload
{
    public AddAuthorPayload(Author author)
    {
        Author = author;
    }

    public Author Author { get; set; }
}
