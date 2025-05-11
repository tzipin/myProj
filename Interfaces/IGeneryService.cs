using myProj.Models;

namespace myProj.interfaces;

public interface IGeneryService<T> where T : Genery
{
    List<T> Get();

    T GetOne(int id);

    int Insert(T newItem);

    int Update(T newItem, int id);

    int Delete(int id);
}
