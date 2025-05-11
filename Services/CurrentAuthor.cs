using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Services;

public class CurrentAuthor 
{   
    public static Author currentAuthor;
    public CurrentAuthor(Author currentAuthor)
    {
        CurrentAuthor.currentAuthor = currentAuthor;
    }

    public static Author GetCurrentAuthor()
    {
        return currentAuthor;
    }
    
    
    // public Author MakeCurrentAuthor(string token)
    // {
    //     int id = TokenServise.GetAuthorIdByToken(token);
    //     if(id == -1 || id == null)
    //         return null;
    //     currentAuthor = AuthorService.Value.GetOne(id);
    //     return currentAuthor;
    // }
    
              
}
