using System.Text.Json;
using myProj.interfaces;
using myProj.Models;


namespace myProj.Services;

public class BookService : GeneryService<Book>, IBookService
{
    public BookService(IHostEnvironment env) : base(env, "Book.json")
    {

    }
    public List<Book> GetBooks()
    {
        if(CurrentAuthor.currentAuthor == null)
        {
            return null;
        }
        if(CurrentAuthor.currentAuthor.Level == 2)
        {
            return Get();
        }            
        else
            return BooksOfAuthor(CurrentAuthor.currentAuthor.Id);
    }
    public Book GetBook(int id)
    {
        Book book = GetOne(id);
        System.Console.WriteLine(book.BookName);
        if(CurrentAuthor.currentAuthor.Level == 2)
        {
            return book;
        }
        if(CurrentAuthor.currentAuthor.Id == book.AuthorId)
        {
            return book;
        }
        return null;
    }
    public override int Insert(Book newBook)
    {
        if(IsItemEmpty(newBook))
            return -1;        
        newBook.Id = list.Count()+1;
        if(CurrentAuthor.currentAuthor.Level == 1)
            newBook.AuthorId = CurrentAuthor.currentAuthor.Id;
        list.Add(newBook);
        saveToFile();
        return newBook.Id;
    }

    public override int Update(Book newBook, int id)
    {
        
        if(CurrentAuthor.currentAuthor.Level == 1 && CurrentAuthor.currentAuthor.Id != newBook.AuthorId)
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
        Book book = GetOne(id);
        if(book == null)
            return -1;
        if(CurrentAuthor.currentAuthor.Level == 1 && CurrentAuthor.currentAuthor.Id != book.AuthorId)
            return -2;
        return base.Delete(id);
    }

    public List<Book> BooksOfAuthor(int authorId)
    {
        System.Console.WriteLine("books count"+Get().FindAll(b => b.AuthorId == authorId).Count);
        return Get().FindAll(b => b.AuthorId == authorId);
    } 
}