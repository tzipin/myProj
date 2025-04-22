using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class AuthorController : ControllerBase
{

    private IGeneryService<Author> authorService;

    public AuthorController(IGeneryService<Author> authorService)
    {
        this.authorService = authorService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Author>> Get()
    {
        return authorService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Author> Get(int id)
    {
        var author = authorService.GetOne(id);
        if(author == null)
            return NotFound();
        return author;
    }

    [HttpPost]
    public ActionResult Post(Author newAuthor)
    {
        var newId = authorService.Insert(newAuthor);
        if(newId == -1){
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new{Id = newId});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Author newAuthor)
    {
        if(!authorService.Update(newAuthor, id))
            return BadRequest();        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if(!authorService.Delete(id))
            return NotFound();
        return Ok();
    }
}
