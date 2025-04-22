using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class BookController : ControllerBase
{

    private IGeneryService<Book> bookService;

    public BookController(IGeneryService<Book> bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        return bookService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = bookService.GetOne(id);
        if(book == null)
            return NotFound();
        return book;
    }

    [HttpPost]
    public ActionResult Post(Book newItem)
    {
        var newId = bookService.Insert(newItem);
        if(newId == -1){
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new{Id = newId});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book newItem)
    {
        if(!bookService.Update(newItem, id))
            return BadRequest();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if(!bookService.Delete(id))
            return NotFound();
        return Ok();
    }
}
