using Microsoft.EntityFrameworkCore;
using PublisherDomain;
using Microsoft.Extensions.Logging;

namespace PublisherData;

public class PubContext : DbContext
{
    StreamWriter _writer = new StreamWriter("EFCoreLog.txt", append: true);
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubDatabase")
            .LogTo(_writer.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }); //LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "Rhoda", LastName = "Lerman" });

        var authorList = new Author[]
        {
            new Author { Id = 2, FirstName = "Ruth", LastName = "Ozeki" },
            new Author { Id = 3, FirstName = "Sofia", LastName = "Segovia" },
            new Author { Id = 4, FirstName = "Ursula K.", LastName = "LeGuin" },
            new Author { Id = 5, FirstName = "Hugh", LastName = "Howey" },
            new Author { Id = 6, FirstName = "Isabelle", LastName = "Allende" },
        };

        modelBuilder.Entity<Author>().HasData(authorList);

        var someBooks = new Book[]
        {
            new Book { BookId = 1, AuthorId = 1, Title = "In God's Ear", PublishDate = new DateTime(1989,3,1) },
            new Book { BookId = 2, AuthorId = 2, Title = "A Tale For the Time Being", PublishDate = new DateTime(2013,12,31) },
            new Book { BookId = 3, AuthorId = 3, Title = "The Left Hand of Darkness", PublishDate = new DateTime(1969,3,1) },
        };

        modelBuilder.Entity<Book>().HasData(someBooks);

        // this is optional as AuthorId follows the convention
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId) // Can be replaced with a key that doesn't follow the naming convention
                                            //.HasForeignKey("AuthorId")
            .IsRequired(false);
    }

    public override void Dispose()
    {
        _writer.Dispose();
        base.Dispose();
    }
}
