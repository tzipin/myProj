using System.Text.Json;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public class BookService : IBookService
{
    List<Book>? books { get; }
    private static string fileName = "Book.json";
    private static string filePath;
    public BookService(IHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

        using (var jsonFile = File.OpenText(filePath))
        {
            books = JsonSerializer.Deserialize<List<Book>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
    private void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(books));
    }

    public List<Book> GetBooks() => books;
    public Book GetBook(int id) => books.FirstOrDefault(b => b.Id == id);
    public int Insert(Book newBook)
    {
        if(IsBookEmpty(newBook))
            return -1;        
        newBook.Id = books.Count()+1;
        books.Add(newBook);
        saveToFile();
        return newBook.Id;
    }

    public bool Update(Book newBook, int id)
    {
        if(IsBookEmpty(newBook) || newBook.Id != id)
            return false;
        var index = books.FindIndex(b => b.Id == id);
        if(index == -1)
            return false;
        books[index] = newBook;
        saveToFile();
        return true;
    }

    public bool Delete(int id)
    {
        var book = GetBook(id);
        if(book == null)
            return false;
        books.Remove(book);
        saveToFile();
        return true;
    }

    public bool IsBookEmpty(Book book)
    {
        return book == null;
    }
}

public static class BookUtilities
{
    public static void AddBookConst(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookService>();
    }
}
