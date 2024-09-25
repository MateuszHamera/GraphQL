using GraphQL.Server.Models;

namespace GraphQL.Server.Subscriptions;

public class Subscription
{
    [Subscribe]
    [Topic]
    public Book OnBookPriceUpdated([EventMessage] Book book) => book;
}
