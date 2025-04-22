using System.Text.Json;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public class BookService : GeneryService<Book>
{
    public BookService(IHostEnvironment env) : base(env, "Book.json")
    {
        
    }
    public override int Insert(Book newBook)
    {
        if(IsItemEmpty(newBook))
            return -1;        
        newBook.Id = list.Count()+1;
        list.Add(newBook);
        saveToFile();
        return newBook.Id;
    }

    public override bool Update(Book newBook, int id)
    {
        if(IsItemEmpty(newBook) || newBook.Id != id)
            return false;
        var index = list.FindIndex(b => b.Id == id);
        if(index == -1)
            return false;
        list[index] = newBook;
        saveToFile();
        return true;
    }
}