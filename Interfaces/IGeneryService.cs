using myProj.Models;

namespace myProj.interfaces;

public interface IGeneryService<T> where T : Genery
{
    List<T> Get();

    T GetOne(int id);

    int Insert(T newItem);

    bool Update(T newItem, int id);

    bool Delete(int id);
}
