using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Services;

public class CurrentAuthor : ICurrentAuthor
{
    public Author currentAuthor { get; set; }
    private IGeneryService<Author> authorService;
    public CurrentAuthor(IGeneryService<Author> authorService)
    {
        this.authorService = authorService;
    }
    public Author MakeCurrentAuthor(string token)
    {
        int id = TokenServise.GetAuthorIdByToken(token);
        if(id == -1 || id == null)
            return null;
        currentAuthor = authorService.GetOne(id);
        return currentAuthor;
    }           
}
