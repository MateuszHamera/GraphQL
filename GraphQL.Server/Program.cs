using GraphQL.Server.Data;
using GraphQL.Server.Mutations;
using GraphQL.Server.Queries;
using GraphQL.Server.Services;
using GraphQL.Server.Subscriptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],  
        ValidAudience = builder.Configuration["Jwt:Audience"],  
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))  
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"))
    .AddPolicy("UserPolicy", policy => policy.RequireRole("user", "admin"));


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

builder.Services.AddSingleton<AuthService>();

builder.Services
    .AddDbContextPool<AppDbContext>(options =>
    options.UseSqlite("Data Source=books.db"));

builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints?.MapGraphQL();
});

// Endpoint to get list of authors
app.MapGet("/api/authors", async (AppDbContext context) =>
{
    return await context.Authors.ToListAsync();
});

// Endpoint to get books for a specific author by author ID
app.MapGet("/api/authors/{id}/books", async (int id, AppDbContext context) =>
{
    var books = await context.Books
        .Where(book => book.AuthorId == id)
        .ToListAsync();

    return books.Count > 0 ? Results.Ok(books) : Results.NotFound("Books not found for this author.");
});

app.Run();

