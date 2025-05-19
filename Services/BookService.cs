using System.Text.Json;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public class BookService : GeneryService<Book>, IBookService
{
    // private readonly CurrentAuthor currentAuthor;
    public BookService(IHostEnvironment env, CurrentAuthor currentAuthor) : base(env, "Book.json")
    {
        // this.currentAuthor = currentAuthor;
    }
    public List<Book> GetBooks()
    {
        if(CurrentAuthor.GetCurrentAuthor() == null)
        {
            System.Console.WriteLine("current author is null");
            return null;
        }
        if(CurrentAuthor.GetCurrentAuthor().Level == 2)
        {
            return Get();
        }            
        else
        {
            System.Console.WriteLine("level = 1");
            System.Console.WriteLine(CurrentAuthor.GetCurrentAuthor().Id);
            return BooksOfAuthor(CurrentAuthor.GetCurrentAuthor().Id);

        }
    }
    public Book GetBook(int id)
    {
        Book book = GetOne(id);
        System.Console.WriteLine(book.BookName);
        if(CurrentAuthor.GetCurrentAuthor().Level == 2)
        {
            return book;
        }
        if(CurrentAuthor.GetCurrentAuthor().Id == book.AuthorId)
        {
            return book;
        }
        return null;
    }
    public override int Insert(Book newBook)
    {
        System.Console.WriteLine("start insert book");
        System.Console.WriteLine(newBook.BookName);
        System.Console.WriteLine(newBook.AuthorId);        
        if(IsItemEmpty(newBook))
            return -1;        
        newBook.Id = list.Count()+1;
        if(CurrentAuthor.GetCurrentAuthor().Level == 1)
            newBook.AuthorId = CurrentAuthor.GetCurrentAuthor().Id;
        list.Add(newBook);
        saveToFile();
        return newBook.Id;
    }

    public override int Update(Book newBook, int id)
    {
        System.Console.WriteLine("start update book");
        System.Console.WriteLine(newBook.BookName);
        System.Console.WriteLine(newBook.AuthorId);
        System.Console.WriteLine(CurrentAuthor.GetCurrentAuthor().Id);
        if(CurrentAuthor.GetCurrentAuthor().Level == 1 && CurrentAuthor.GetCurrentAuthor().Id != newBook.AuthorId)
            return -2;
        if(IsItemEmpty(newBook) || newBook.Id != id)
            return -1;
        var index = list.FindIndex(b => b.Id == id);
        if(index == -1)
            return -1;
        list[index] = newBook;
        saveToFile();
        return 1;
    }

    public int Delete(int id)
    {
        System.Console.WriteLine("start delete book");
        Book book = GetOne(id);
        if(book == null)
            return -1;
        System.Console.WriteLine("book id : "+book.Id);
        System.Console.WriteLine("book author id : "+book.AuthorId);
        System.Console.WriteLine("current author id : "+CurrentAuthor.GetCurrentAuthor().Id);
        if(CurrentAuthor.GetCurrentAuthor().Level == 1 && CurrentAuthor.GetCurrentAuthor().Id != book.AuthorId)
            return -2;
        return base.Delete(id);
    }

    public List<Book> BooksOfAuthor(int authorId)
    {
        System.Console.WriteLine("books of author");
        System.Console.WriteLine("books count"+Get().FindAll(b => b.AuthorId == authorId).Count);
        return Get().FindAll(b => b.AuthorId == authorId);
    } 
}