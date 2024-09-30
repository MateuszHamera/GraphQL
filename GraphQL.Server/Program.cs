using GraphQL.Server.Data;
using GraphQL.Server.Mutations;
using GraphQL.Server.Queries;
using GraphQL.Server.Subscriptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Blazor-Client",
        builder => builder
            .WithOrigins("https://localhost:7148") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services
    .AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=books.db"));

builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddSorting()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();

app.UseCors("Blazor-Client");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints?.MapGraphQL();
});

app.MapGet("authors", (AppDbContext context) => {
    Query query = new Query();
    var result = query.GetAuthors(context);

    return result;
});

app.Run();

