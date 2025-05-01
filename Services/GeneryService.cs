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
    public virtual List<T> Get() => list;
    public virtual T GetOne(int id) => list.FirstOrDefault(b => b.Id == id);
    public abstract int Insert(T newItem);
    public abstract bool Update(T newItem, int id);
    public virtual bool Delete(int id)
    {
        var item = GetOne(id);
        if(item == null)
            return false;
        list.Remove(item);
        saveToFile();
        return true;
    }
    public bool IsItemEmpty(T item)
    {
        return item == null;
    }  


}

public static class GeneryUtilities
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IGeneryService<Book>, BookService>();
        services.AddSingleton<IGeneryService<Author>, AuthorService>();
        services.AddSingleton<AuthorService>();
        services.AddSingleton<BookService>();
        services.AddSingleton<ICurrentAuthor,CurrentAuthor>();

    }
}