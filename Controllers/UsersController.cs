using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{

    private IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        return userService.GetUsers();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = userService.GetUser(id);
        if(user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = userService.Insert(newUser);
        if(newId == -1){
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new{Id = newId});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, User newUser)
    {
        if(!userService.Update(id, newUser))
            return BadRequest();        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if(!userService.Delete(id))
            return NotFound();
        return Ok();
    }
}
