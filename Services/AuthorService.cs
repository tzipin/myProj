using System.Security.Claims;
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
        newAuthor.Id = list.LastOrDefault().Id+1;
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
    

    public int GetAuthorByNameAndPassword(string name, string password)
    {
        Author author = Get().Find(a => a.Name == name && a.Password == password);
        if(author == null)
            return -1;
        return author.Id;
    }

    public string Login(Author author)
    {
        int id = GetAuthorByNameAndPassword(author.Name, author.Password);
        if(id == -1)
            return null;
        string type, level;
        if(author.Name == "Tzipi" || author.Password == "t1234"){
            type = "Librarian";
            level = "2";
        }            
        else
        {
            type = "Author";
            level = "1";
        }
        var claims = new List<Claim>()
        {
            new Claim("type", type),
            new Claim("Level", level),
            new Claim("Id", id.ToString()),
        } ;  
        var token = TokenServise.GetToken(claims);
        string stringToken = TokenServise.WriteToken(token);
        return stringToken;     
    }
}