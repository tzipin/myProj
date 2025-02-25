using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class BookCntroller : ControllerBase
{

    private IBookService bookService;

    public BookCntroller(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        return bookService.GetBooks();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = bookService.GetBook(id);
        if(book == null)
            return NotFound();
        return book;
    }

    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        var newId = bookService.Insert(newBook);
        if(newId == -1)
            return BadRequest();

        return CreatedAtAction(nameof(Post), new{Id = newId});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Book newBook)
    {
        if(!bookService.Update(id, newBook))
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
