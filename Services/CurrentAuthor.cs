using Microsoft.AspNetCore.Mvc;
using myProj.interfaces;
using myProj.Models;

namespace myProj.Services;

public class CurrentAuthor
{
    private static Author currentAuthor { get; set; }

    public static void SetCurrentAuthor(Author author)
    {
        currentAuthor = author;
        // System.Console.WriteLine("set current author "+currentAuthor.Name);

    }

    public static Author GetCurrentAuthor()
    {
        return currentAuthor;
    }
}