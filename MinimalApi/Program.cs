using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApi;
using MinimalApi.Constants;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBookService, BookService>();

//builder.Services.AddValidatorsFromAssemblyContaining<GithubIssueValidator>();
builder.Services.AddScoped<IGithubIssuesRouteHandler, GithubIssuesRouteHandler>();

// Adding rate limiter and policy
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy(PolicyNames.AuthenticatedUserPolicy, AuthenticatedUserPolicy.Instance);
});


var app = builder.Build();

// Using rate limiter
app.UseRateLimiter();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Hello Word
app.MapGet("/", () => "Hello, World!");

// Example 1: Get all books
app.MapGet("/books", (IBookService bookService) =>
    TypedResults.Ok(bookService.GetBooks()))
    .WithName("GetBooks");

// Example 2: Get a specific book by ID
app.MapGet("/books/{id}", Results<Ok<Book>, NotFound> (IBookService bookService, int id) =>
{
    var book = bookService.GetBook(id);
    return book is { } ? TypedResults.Ok(book) : TypedResults.NotFound();
}).WithName("GetBookById");

// Example 3: Add a new book
app.MapPost("/books", (IBookService bookService, Book newBook) =>
{
    bookService.AddBook(newBook);
    return TypedResults.Created($"/books/{newBook.Id}", newBook);
}).WithName("AddBook");

// Example 4: Update an existing book
app.MapPut("/books/{id}", (IBookService bookService, int id, Book updatedBook) =>
{
    bookService.UpdateBook(id, updatedBook);
    return TypedResults.Ok();
}).WithName("UpdateBook");

// Example 5: Delete a book by ID
app.MapDelete("/books/{id}", (IBookService bookService, int id) =>
{
    bookService.DeleteBook(id);
    return TypedResults.NoContent();
}).WithName("DeleteBook");

// Grouping routes
app.MapGroup(Routes.BaseRoute)
    .MapGitHubIssuesRoutes(builder.Services)
    .RequireRateLimiting(PolicyNames.AuthenticatedUserPolicy);

app.Run();

public partial class Program { }