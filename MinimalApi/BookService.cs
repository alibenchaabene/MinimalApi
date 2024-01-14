namespace MinimalApi
{
    public interface IBookService
    {
        List<Book> GetBooks();

        Book GetBook(int id);
    }

    public class BookService : IBookService
    {
        private readonly List<Book> _books;

        public BookService()
        {
            _books = new List<Book>
            {
               new Book
               {
                   Id = 1,
                   Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                   Author = "Robert C. Martin"
               },
                new Book
                {
                    Id = 2,
                    Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                    Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides"
                },
                new Book
                {
                    Id = 3,
                    Title = "Refactoring: Improving the Design of Existing Code",
                    Author = "Martin Fowler"
                },
                new Book
                {
                    Id = 4,
                    Title = "Code Complete: A Practical Handbook of Software Construction",
                 Author = "Steve McConnell"
                }

            };
        }

        public List<Book> GetBooks()
        {
            return this._books;
        }

        public Book GetBook(int id)
        {
            var book = this._books.FirstOrDefault(x => x.Id == id);

            return book;
        }
    }

    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}

