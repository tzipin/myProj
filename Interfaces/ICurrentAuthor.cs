using myProj.Models;

namespace myProj.interfaces;

public interface ICurrentAuthor
{
    Author currentAuthor { get; set; }
    Author MakeCurrentAuthor(string token);
}