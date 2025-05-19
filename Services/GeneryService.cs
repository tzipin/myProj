using System.Runtime.InteropServices;
using System.Text.Json;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public abstract class GeneryService<T> : IGeneryService<T> where T : Genery 
{
    protected List<T>? list { get; }
    private static string fileName;
    private static string filePath;
    public GeneryService(IHostEnvironment env, string fName)
    {
        fileName = fName;
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

        using (var jsonFile = File.OpenText(filePath))
        {
            list = JsonSerializer.Deserialize<List<T>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    protected void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(list));
    }
    public virtual List<T> Get(){ return list;}
    public virtual T GetOne(int id) => list.FirstOrDefault(b => b.Id == id);
    public abstract int Insert(T newItem);
    public abstract int Update(T newItem, int id);
    public virtual int Delete(int id)
    {
        var item = GetOne(id);
        if(item == null)
            return -1;
        list.Remove(item);
        saveToFile();
        return id;
    }
    public bool IsItemEmpty(T item)
    {
        return item == null;
    }  

    public static List<Book> BooksOfAuthor(List<Book> list, int authorId)
    {
        List<Book> books = list.FindAll(b => b.AuthorId == authorId);
        return books;
    }
}

public static class GeneryUtilities
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGeneryService<Book>, BookService>();
        services.AddScoped<IGeneryService<Author>, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<BookService>();
        services.AddScoped<Author>();
        services.AddScoped<CurrentAuthor>();
    }
}
