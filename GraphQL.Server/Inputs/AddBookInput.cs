using GraphQL.Server.Models;

namespace GraphQL.Server.Inputs;

public record AddBookInput(string title, string description, double price, int authorId);

