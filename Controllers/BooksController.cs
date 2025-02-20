using Microsoft.AspNetCore.Mvc;
using myProj.Models;
using myProj.Services;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class BookCntroller : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        return BookService.GetBooks();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = BookService.GetBook(id);
        if(book == null)
            return NotFound();
        return book;
    }

    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        var newId = BookService.Insert(newBook);
        if(newId == -1)
            return BadRequest();

        return CreatedAtAction(nameof(Post), new{Id = newId});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book newBook)
    {
        if(!BookService.Update(id, newBook))
            return BadRequest();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if(!BookService.Delete(id))
            return NotFound();

        return Ok();
    }
}
