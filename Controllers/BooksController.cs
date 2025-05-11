using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using myProj.Models;
using myProj.Services;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private BookService bookService;
    public BookController(BookService bookService) => this.bookService = bookService;

    [HttpGet]
    [Authorize(Policy = "Author")]
    public ActionResult<IEnumerable<Book>> Get()
    {
        var books = bookService.GetBooks();
        if(books == null)
            return Unauthorized();
        if(books.Count == 0)
            return NoContent();
        return books;
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = bookService.GetBook(id);
        if(book == null)
            return Unauthorized();
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
        int req = bookService.Update(newItem, id);
        if(req == -1)
            return BadRequest();   
        if(req == -2)
            return Unauthorized();     
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        int req = bookService.Delete(id);
        if(req == -1)
            return NotFound();
        if(req == -2)
            return Unauthorized();
        return Ok();
    }
}
