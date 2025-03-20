using myProj.Models;

namespace myProj.interfaces;

public interface IBookService
{
    List<Book> GetBooks();

    Book GetBook(int id);

    int Insert(Book newBook);

    bool Update(Book newBook, int id);

    bool Delete(int id);
}
