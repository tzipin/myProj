using myProj.Models;

namespace myProj.interfaces;

public interface IBookService
{
    List<Book> GetBooks();

    Book GetBook(int id);

    int Insert(Book newBook);

    bool Update(int id, Book newBook);

    bool Delete(int id);
}
