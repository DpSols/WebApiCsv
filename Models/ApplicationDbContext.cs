using Microsoft.EntityFrameworkCore;

namespace WebApiCsv.Models;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<Pet> Pets { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=ITechArtIntern;Trusted_Connection=True;");
    }
}
