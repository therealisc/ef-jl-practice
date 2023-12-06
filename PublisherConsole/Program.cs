using PublisherData;
using PublisherDomain;
using Microsoft.EntityFrameworkCore;


//dotnet ef migrations add
//

//using (PubContext context = new PubContext())
//{
//    context.Database.EnsureCreated();
//}

//GetAuthors();
//AddAuthor();
//GetAuthors();
//AddAuthorWithBook();
//GetAuthorsWithBooks();

var _context = new PubContext();

void SortAuthors()
{
    var authorsByLastName = _context.Authors.AsNoTracking()
        .OrderBy(a => a.LastName)
        .ThenBy(a => a.FirstName).ToList();

    authorsByLastName.ForEach(a => Console.WriteLine(a.LastName + " | " + a.FirstName));
}
SortAuthors();

void QueryFilters()
{
    var name = "Julie";
    //var authors = _context.Authors.Where(x => EF.Functions.Like(x.FirstName, "J%")).ToList();
    var author = _context.Authors.Find(1);
    //foreach (var author in authors)
    //{
    Console.WriteLine(author.FirstName + " " + author.LastName);
    //}
}

QueryFilters();

void AddSomeMoreAuthors()
{
    _context.Authors.Add(new Author { FirstName = "Ronds", LastName = "Lerman" });
    _context.Authors.Add(new Author { FirstName = "Don", LastName = "Jones" });
    _context.Authors.Add(new Author { FirstName = "Jim", LastName = "Christopher" });
    _context.Authors.Add(new Author { FirstName = "Stephen", LastName = "Hans" });
    _context.SaveChanges();
}

//AddSomeMoreAuthors();

void SkipAndTakeAuthors()
{
    var groupSize = 2;
    for (int i = 0; i < 5; i++)
    {
        var authors = _context.Authors.Skip(groupSize * i).Take(groupSize).ToList();
        Console.WriteLine($"Group {i}:");
        foreach (var author in authors)
        {
            Console.WriteLine(author.FirstName + " " + author.LastName);
        }
    }
}

SkipAndTakeAuthors();

void AddAuthorWithBook()
{
    var author = new Author { FirstName = "Julie", LastName = "Lerman" };
    author.Books.Add(new Book { Title = "Programming Entity Framework", PublishDate = new DateTime(2009, 1, 1) });

    author.Books.Add(new Book { Title = "Programming Entity Framework 2nd Ed", PublishDate = new DateTime(2010, 8, 1) });

    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthorsWithBooks()
{
    using var context = new PubContext();
    var authors = context.Authors.Include(a => a.Books).ToList();
    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);
        foreach (var book in author.Books)
        {
            Console.WriteLine("*" + book.Title);
        }
    }
}

void AddAuthor()
{
    var author = new Author { FirstName = "Stupid", LastName = "John" };
    using var context = new PubContext();
    context.Authors.Add(author);
    context.SaveChanges();
}

void GetAuthors()
{
    using var context = new PubContext();
    var authors = context.Authors.ToList();
    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);
    }
}
