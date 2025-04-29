using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using myProj.interfaces;
using myProj.Models;
using myProj.Services;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class BookController : ControllerBase
{

    private BookService bookService;

    public BookController(BookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    // [Authorize(Policy = "Librarian")]
    // [Authorize(Policy = "Author")]
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
