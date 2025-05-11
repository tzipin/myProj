using System.Security.Claims;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;
public class AuthorService : GeneryService<Author>, IAuthorService
{
    BookService bookService;
    public AuthorService(IHostEnvironment env, BookService bookService) : base(env, "Author.json")
    {
        this.bookService = bookService;
    }

    public override List<Author> Get()
    {
    System.Console.WriteLine(CurrentAuthor.currentAuthor.Level);
        if(CurrentAuthor.currentAuthor.Level == 1)
            return list.FindAll(a => a.Id == CurrentAuthor.currentAuthor.Id);
        return list;
    }

    public Author GetAuthor(int id)
    {
        Author author = GetOne(id);
        if(CurrentAuthor.currentAuthor.Level == 1 && CurrentAuthor.currentAuthor.Id != id)
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
        if(IsItemEmpty(newAuthor) || newAuthor.Id != id)
            return -1;
        var index = list.FindIndex(b => b.Id == id);
        if(index == -1)
            return -1;
        if(CurrentAuthor.currentAuthor.Level == 1 && CurrentAuthor.currentAuthor.Level != 1)
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
        System.Console.WriteLine("in GetAuthorByNameAndPassword");
        Author author = base.Get().Find(a => {System.Console.WriteLine(a.Name+" "+ a.Password); return a.Name == name && a.Password == password;});
        if(author == null){
            System.Console.WriteLine("author is null");
            return null;
        }
        System.Console.WriteLine("author id : ",author.Id);
        return author;
    }

    public string Login(Author author)
    {
        System.Console.WriteLine(author.Name, author.Password);
        Author currentAuthor = GetAuthorByNameAndPassword(author.Name, author.Password);
        if(currentAuthor == null){
            System.Console.WriteLine("current author is null");
            return null;
        }
        string type, level;
        if(author.Name == "Tzipi" && author.Password == "t1234")
        {
            System.Console.WriteLine("author is librarian");
            type = "Librarian";
            level = "2";
        }            
        else
        {
            System.Console.WriteLine("author is author");
            type = "Author";
            level = "1";
        }
        var claims = new List<Claim>()
        {
            new Claim("type", type),
            new Claim("Level", level),
            new Claim("Id", currentAuthor.Id.ToString()),
        } ;  
        var token = TokenServise.GetToken(claims);
        CurrentAuthor current = new CurrentAuthor(currentAuthor);
        string stringToken = TokenServise.WriteToken(token);
        return stringToken;     
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