namespace myProj.Models;

public class Book : Genery
{
    public string? BookName { get; set; }
    public bool? IsOnlyAdults { get; set; }
    public int AuthorId { get; set; }
    
}