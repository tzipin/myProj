using Microsoft.AspNetCore.Mvc;
using myProj.Models;
using myProj.Services;
using Microsoft.AspNetCore.Authorization;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private AuthorService authorService;
    public AuthorController(AuthorService authorService)
    {
        this.authorService = authorService;
    }

    [HttpGet]
    // [Authorize(Policy = "Librarian")]
    public ActionResult<IEnumerable<Author>> Get()
    {
        return authorService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "Level2")]
    public ActionResult<Author> GetOne(int id)
    {
        var author = authorService.GetAuthor(id);
        if(author == null)
            return NotFound();
        if(author.Id == -1)
            return Unauthorized();
        return author;
    }

    [HttpPost]
    [Authorize(Policy = "Librarian")]
    public ActionResult<string> Joining([FromBody] Author newAuthor)
    {
        System.Console.WriteLine("start joining");
        var newId = authorService.Insert(newAuthor);
        if(newId == -1)
            return BadRequest();
        return Login(newAuthor);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,[FromBody] Author newAuthor)
    {
        if(CurrentAuthor.currentAuthor.Level == 1 && CurrentAuthor.currentAuthor.Id != id)
            return Unauthorized();
        int req = authorService.Update(newAuthor, id);
        if(req == -1)
            return BadRequest();   
        if(req == -2)
            return Unauthorized();     
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Librarian")]
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
        System.Console.WriteLine(token);
        if(token == null)
            return BadRequest("Wrong name or password");
        return Ok(token);      
    }

}
