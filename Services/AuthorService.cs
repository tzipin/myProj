using System.Text.Json;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Services;

public class AuthorService : GeneryService<Author>
{
    public AuthorService(IHostEnvironment env) : base(env, "Author.json")
    {
        
    }
    public override int Insert(Author newAuthor)
    {
        if(IsItemEmpty(newAuthor))
            return -1;        
        newAuthor.Id = list.Count()+1;
        list.Add(newAuthor);
        saveToFile();
        return newAuthor.Id;
    }

    public override bool Update(Author newAuthor, int id)
    {
        if(IsItemEmpty(newAuthor) || newAuthor.Id != id)
            return false;
        var index = list.FindIndex(b => b.Id == id);
        if(index == -1)
            return false;
        list[index] = newAuthor;
        saveToFile();
        return true;
    }
}