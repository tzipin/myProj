using System.Text.Json;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public class BookService : GeneryService<Book>
{
    private ICurrentAuthor currentAuthor;
    //private static List<Book> list = new List<Book>();
    public BookService(IHostEnvironment env, ICurrentAuthor currentAuthor) : base(env, "Book.json")
    {
        this.currentAuthor = currentAuthor;
    }
    public List<Book> GetBooks(string token)
    {
        if(currentAuthor.MakeCurrentAuthor(token) == null)
            return null;
        if(currentAuthor.currentAuthor.Level == 1)
            return Get();
        else
            return list.FindAll(b => b.AuthorId == currentAuthor.currentAuthor.Id);;
    }
    public override Book GetOne(int id)
    {
        return list.Find(b => b.Id == id);
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

    // public static List<Book> booksOfAuthor(int authorId)
    // {
    //     System.Console.WriteLine(list.Count());
    //     List<Book> books = list.FindAll(b => b.AuthorId == authorId);
    //     System.Console.WriteLine(books.Count());
    //     return books;
    // }
}