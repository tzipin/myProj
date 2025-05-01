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
    public ActionResult<string> Joining(Author newAuthor)
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
        //BookService.booksOfAuthor(id).ForEach(b => bookService.Delete(b.Id));
        authorService.Delete(id);
        return Ok();
    }

    [HttpPost]
    [Route("/login")]
    public ActionResult<string> Login([FromBody] Author author)
    {
        string token = authorService.Login(author);
        System.Console.WriteLine("token: " + token);
        if(token == null)
            return BadRequest("Wrong name or password");
        return Ok(token);      
    }

}
