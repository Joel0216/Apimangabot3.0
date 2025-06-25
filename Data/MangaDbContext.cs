using Microsoft.EntityFrameworkCore;
using JaveragesLibrary.Domain.Entities;
namespace JaveragesLibrary.Data
{
    public class MangaDbContext : DbContext
    {
        public MangaDbContext(DbContextOptions<MangaDbContext> options)
            : base(options) { }

        public DbSet<Manga> Mangas { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
    }
}