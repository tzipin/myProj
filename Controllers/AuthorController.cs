using Microsoft.AspNetCore.Mvc;
using myProj.Models;
using System.Security.Claims;
using myProj.Services;
using Microsoft.AspNetCore.Authorization;
using myProj.interfaces;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthorController : ControllerBase
{

    private AuthorService authorService;
    private IGeneryService<Book> bookService;

    public AuthorController(AuthorService authorService, IGeneryService<Book> bookService)
    {
        this.authorService = authorService;
        this.bookService = bookService;
    }

    // public AuthorController(IGeneryService<Author> authorService)
    // {
    //     this.authorService = authorService;
    // }

    [HttpGet]
    [Authorize(Policy = "Librarian")]
    public ActionResult<IEnumerable<Author>> Get()
    {
        System.Console.WriteLine("start get");
        return authorService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Level2")]
    public ActionResult<Author> GetOne(int id)
    {
        var author = authorService.GetOne(id);
        if(author == null)
            return NotFound();
        return author;
    }

    [HttpPost]
    [Authorize(Policy = "Librarian")]
    public ActionResult<String> Joining(Author newAuthor)
    {
        var newId = authorService.Insert(newAuthor);
        if(newId == -1)
            return BadRequest();
        return Login(newAuthor);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Author newAuthor)
    {
        if(!authorService.Update(newAuthor, id))
            return BadRequest();        
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Librarian")]
    public ActionResult Delete(int id)
    {
        if(authorService.GetOne(id) == null)
            return NotFound();
        authorService.booksOfAuthor(id).ForEach(b => bookService.Delete(b.Id));
        authorService.Delete(id);
        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public ActionResult<String> Login([FromBody] Author author)
    {
        int id = authorService.GetAuthorByNameAndPassword(author.Name, author.Password);
        if(id == -1)
            return "user not found";
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
        HttpContext.Session.SetString("token", TokenServise.WriteToken(token));
        return new OkObjectResult(TokenServise.WriteToken(token));        
    }

}
