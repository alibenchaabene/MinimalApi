using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//---------------------------- Middlewares -----------------------------//

// Request Logging Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming Request: {context.Request.Protocol} {context.Request.Method} {context.Request.Path}{context.Request.QueryString}");
    await next();
});

// Request Modification Middleware
app.Use(async (context, next) =>
{
    context.Request.Headers.Append("Custom-Header", "Request Modified");

    // Log all headers for demonstration purposes
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"Custom-Header: {context.Request.Headers["Custom-Header"]}");
    }

    await next();
});

// Response Modification Middleware
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        if (!context.Response.Headers.ContainsKey("Custom-Header"))
        {
            context.Response.Headers.Append("Custom-Header", "Response Modified");
        }
        return Task.CompletedTask;
    });

    await next();
});


//---------------------------- Endpoints -----------------------------//

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

app.Run();
