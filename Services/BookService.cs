using Microsoft.AspNetCore.Mvc;
using myProj.Models;
using myProj.Services;

namespace myProj.Services;

public static class BookService
{
    private static List<Book> books;

    static BookService()
    {
        books = new List<Book>
        {
            new Book { Id = 1, BookName = "איסתרק" },
            new Book { Id = 2, BookName = "לחיות וחצי", OnlyAdults = true },
        };
    }

    public static List<Book> GetBooks()
    {
        return books;
    }

    public static Book GetBook(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        return book;
    }

    public static int Insert(Book newBook)
    {
        if(IsBookEmpty(newBook))
            return -1;
        int maxId = books.Max(b => b.Id);
        newBook.Id = maxId + 1;
        books.Add(newBook);
        return newBook.Id;
    }

    public static bool Update(int id, Book newBook)
    {
        if(IsBookEmpty(newBook) || newBook.Id != id)
            return false;

        var book = books.FirstOrDefault(b => b.Id == id);
        if(book == null)
            return false;

        book.BookName = newBook.BookName;
        book.OnlyAdults = newBook.OnlyAdults;
        return true;
    }

    public static bool Delete(int id)
    {
        var book = books.FirstOrDefault(b => b.Id == id);
        if(book == null)
            return false;

        int index = books.IndexOf(book);
        books.RemoveAt(index);
        return true;
    }

    public static bool IsBookEmpty(Book book)
    {
        return book == null || string.IsNullOrWhiteSpace (book.BookName);
    }
}
