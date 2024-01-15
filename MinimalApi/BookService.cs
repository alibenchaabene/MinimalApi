namespace MinimalApi
{
    public interface IBookService
    {
        List<Book> GetBooks();

        Book GetBook(int id);

        void AddBook(Book book);

        void UpdateBook(int id, Book updatedBook);

        void DeleteBook(int id);
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
            return _books;
        }

        public Book GetBook(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }

        public void AddBook(Book book)
        {
            // You may want to add some validation logic before adding the book.
            _books.Add(book);
        }

        public void UpdateBook(int id, Book updatedBook)
        {
            var existingBook = _books.FirstOrDefault(x => x.Id == id);
            if (existingBook != null)
            {
                // You may want to add some validation logic before updating the book.
                existingBook.Title = updatedBook.Title;
                existingBook.Author = updatedBook.Author;
            }
        }

        public void DeleteBook(int id)
        {
            var bookToRemove = _books.FirstOrDefault(x => x.Id == id);
            if (bookToRemove != null)
            {
                _books.Remove(bookToRemove);
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }
    }
}
