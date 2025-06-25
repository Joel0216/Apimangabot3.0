using JaveragesLibrary.Data;
using JaveragesLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JaveragesLibrary.Services.Features.Mangas
{
    public class MangaService
    {
        private readonly MangaDbContext _context;

        public MangaService(MangaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Manga>> GetAllMangasAsync()
        {
            return await _context.Mangas.ToListAsync();
        }

        public async Task<Manga?> GetMangaByIdAsync(int id)
        {
            return await _context.Mangas.FindAsync(id);
        }

        public async Task<Manga> CreateMangaAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task UpdateMangaAsync(Manga manga)
        {
            _context.Entry(manga).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMangaAsync(int id)
        {
            var manga = await _context.Mangas.FindAsync(id);
            if (manga != null)
            {
                _context.Mangas.Remove(manga);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Manga>> SearchMangasByTitleAsync(string titulo)
        {
            return await _context.Mangas
                .Where(m => m.Titulo != null && m.Titulo.Contains(titulo))
                .ToListAsync();
        }

        public async Task<bool> MangaExistsAsync(int id)
        {
            return await _context.Mangas.AnyAsync(m => m.Id == id);
        }
    }
}   