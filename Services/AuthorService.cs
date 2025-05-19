using System.Security.Claims;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;
public class AuthorService : GeneryService<Author>, IAuthorService
{
    BookService bookService;
    // private readonly CurrentAuthor currentAuthor;
    public AuthorService(IHostEnvironment env, BookService bookService, CurrentAuthor currentAuthor) : base(env, "Author.json")
    {
        this.bookService = bookService;
        // this.currentAuthor = currentAuthor;
    }
    // public override List<Author> Get()
    // {
    // System.Console.WriteLine(CurrentAuthor.GetCurrentAuthor().Level);
    //     if(CurrentAuthor.GetCurrentAuthor().Level == 1)
    //         return list.FindAll(a => a.Id == CurrentAuthor.GetCurrentAuthor().Id);
    //     return list;
    // }

    public Author GetAuthor(int id)
    {
        Author author = GetOne(id);
        if(CurrentAuthor.GetCurrentAuthor().Level == 1 && CurrentAuthor.GetCurrentAuthor().Id != id)
            author.Id = -1;
        return author;
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

    public override int Update(Author newAuthor, int id)
    {
        System.Console.WriteLine("start update author");
        if(IsItemEmpty(newAuthor) || newAuthor.Id != id)
            return -1;
        var index = list.FindIndex(b => b.Id == id);
        if(index == -1)
            return -1;
        if(CurrentAuthor.GetCurrentAuthor().Level == 1 && CurrentAuthor.GetCurrentAuthor().Level != 1)
            return -2;
        list[index] = newAuthor;
        saveToFile();
        return 1;
    }

    public override int Delete(int id)
    {
        if(GetOne(id) == null)
            return -1;
        bookService.BooksOfAuthor(id).ForEach(b => bookService.Delete(b.Id));
        base.Delete(id);
        return id;
    }
    

    public Author GetAuthorByNameAndPassword(string name, string password)
    {
        // System.Console.WriteLine("in GetAuthorByNameAndPassword");
        Author author = list!.Find(a => a.Name == name && a.Password == password)!;
        if(author == null){
            // System.Console.WriteLine("author is null");
            return null;
        }
       
        // System.Console.WriteLine("author id : "+author.Id);
        return author;
    }

    public string Login(Author author)
    {
        // System.Console.WriteLine("blabla"+author.Name+ author.Password);
        Author current = GetAuthorByNameAndPassword(author.Name, author.Password);
        if(current == null){
            // System.Console.WriteLine("current author is null");
            return null;
        }
        // System.Console.WriteLine("current author name : "+current.Name);
        string type, level;
        if(author.Name == "Tzipi" && author.Password == "t1234")
        {
            // System.Console.WriteLine("author is Librarian");
            type = "Librarian";
            level = "2";
        }            
        else
        {
            // System.Console.WriteLine("author is author");
            type = "Author";
            level = "1";
        }
        var claims = new List<Claim>()
        {
            new Claim("type", type),
            new Claim("Level", level),
            new Claim("Id", current.Id.ToString()),
        } ;  

        var token = TokenServise.GetToken(claims);
        CurrentAuthor.SetCurrentAuthor(current); 
        string stringToken = TokenServise.WriteToken(token);
        return stringToken +" "+ type+" "+ current.Id;     
    }

    //  public Author MakeCurrentAuthor(string token)
    // {
    //     int id = TokenServise.GetAuthorIdByToken(token);
    //     if(id == -1 || id == null)
    //         return null;
    //     currentAuthor = authorService.GetOne(id);
    //     return currentAuthor;
    // }
}