using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public class BookService : IBookService
{
    private List<Book> books;

    public BookService()
    {
        books = new List<Book>
        {
            new Book { Id = 1, BookName = "איסתרק" },
            new Book { Id = 2, BookName = "לחיות וחצי", OnlyAdults = true },
        };
    }

    public List<Book> GetBooks()
    {
        return books;
    }

    public Book GetBook(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        return book;
    }

    public int Insert(Book newBook)
    {
        if(IsBookEmpty(newBook))
            return -1;
        int maxId = books.Max(b => b.Id);
        newBook.Id = maxId + 1;
        books.Add(newBook);
        return newBook.Id;
    }

    public bool Update(int id, Book newBook)
    {
        if(IsBookEmpty(newBook) || newBook.Id != id)
            return false;

        var book = books.FirstOrDefault(b => b.Id == id);
        if(book == null)
            return false;

        book.BookName = newBook.BookName;
        book.OnlyAdults = newBook.OnlyAdults;
        return true;
    }

    public bool Delete(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if(book == null)
            return false;

        int index = books.IndexOf(book);
        books.RemoveAt(index);
        return true;
    }

    public bool IsBookEmpty(Book book)
    {
        return book == null || string.IsNullOrWhiteSpace (book.BookName);
    }
}

public static class BookUtilities
{
    public static void AddBookConst(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookService>();
    }
}
