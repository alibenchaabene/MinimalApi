using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// example 1
app.MapGet("/", () => "Hello, World!");

// example 2
app.MapGet("/books", (IBookService bookService) =>
    TypedResults.Ok(bookService.GetBooks()))
.WithName("GetBooks");


// example 3
app.MapGet("/books/{id}", Results<Ok<Book>, NotFound> (IBookService bookService, int id) =>
        bookService.GetBook(id) is { } book
            ? TypedResults.Ok(book)
            : TypedResults.NotFound()
    )
    .WithName("GetBookById");

app.Run();