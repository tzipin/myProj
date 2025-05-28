using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using myProj.Models;
using myProj.Services;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private AuthorService authorService;
    public AuthorController(AuthorService authorService, CurrentAuthor currentAuthor)
    {
        this.authorService = authorService;
    }

    [HttpGet]
    [Authorize(Policy = "Librarian")]
    public ActionResult<IEnumerable<Author>> Get()
    {       
        if(CurrentAuthor.GetCurrentAuthor().Level == 1)
            return Unauthorized(); 
        return authorService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Author")]
    public ActionResult<Author> GetOne(int id)
    {
        var author = authorService.GetAuthor(id);
        if(author == null)
            return NotFound();
        if(author.Id == -1){
            return Unauthorized();
        }
        return author;
    }

    [HttpPost]
    [Authorize(policy: "Librarian")]
    public ActionResult<string> Joining([FromBody] Author newAuthor)
    {
        System.Console.WriteLine("start joining");
        var newId = authorService.Insert(newAuthor);
        System.Console.WriteLine(newId);
        if (newId == -1)
            return BadRequest();
        return Login(newAuthor);
    }

    [HttpPut("{id}")]
    [Authorize(policy: "Author")]
    public ActionResult Put(int id,[FromBody] Author newAuthor)
    {
        System.Console.WriteLine("start put author");
        if(CurrentAuthor.GetCurrentAuthor().Level == 1 && CurrentAuthor.GetCurrentAuthor().Id != id)
        {
            System.Console.WriteLine("Unauthorized");
            return Unauthorized();
        }
        int req = authorService.Update(newAuthor, id);
        if(req == -1)
        {
            System.Console.WriteLine("BadRequest");
            return BadRequest();
        }   
        if(req == -2)
        {
            System.Console.WriteLine("unauthorized");
            return Unauthorized();    
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(policy: "Librarian")]
    public ActionResult Delete(int id)
    {
        if(authorService.Delete(id) == -1)
            return NotFound();
        return Ok();
    }

    [HttpPost]
    [Route("/login")]
    public ActionResult<string> Login([FromBody] Author author)
    {
        System.Console.WriteLine("start login");
        string token = authorService.Login(author);
        if(token == null)
            return BadRequest("Wrong name or password");
        return Ok(token);      
    }

}
