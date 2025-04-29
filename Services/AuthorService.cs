using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using myProj.Models;


namespace myProj.Services;

public class AuthorService : GeneryService<Author>
{
    private BookService bookService;
    public AuthorService(IHostEnvironment env, BookService bookService) : base(env, "Author.json")
    {
        this.bookService = bookService;
    }
    public override int Insert(Author newAuthor)
    {
        if(IsItemEmpty(newAuthor))
            return -1;        
        newAuthor.Id = list.LastOrDefault().Id+1;
        list.Add(newAuthor);
        saveToFile();
        return newAuthor.Id;
    }

    public override bool Update(Author newAuthor, int id)
    {
        if(IsItemEmpty(newAuthor) || newAuthor.Id != id)
            return false;
        var index = list.FindIndex(b => b.Id == id);
        if(index == -1)
            return false;
        list[index] = newAuthor;
        saveToFile();
        return true;
    }
    public List<Book> booksOfAuthor(int authorId)
    {
        List<Book> books =  bookService.Get();
        books = books.FindAll(b => b.AuthorId == authorId);
        return books;
    }

    public int GetAuthorByNameAndPassword(String name, String password)
    {
        Author author = Get().Find(a => a.Name == name && a.Password == password);
        if(author == null)
            return -1;
        return author.Id;
    }
}