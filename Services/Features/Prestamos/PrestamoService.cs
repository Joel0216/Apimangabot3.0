using JaveragesLibrary.Data;
using JaveragesLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JaveragesLibrary.Services.Features.Prestamos
{
    public class PrestamoService
    {
        private readonly MangaDbContext _context;

        public PrestamoService(MangaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prestamo>> GetAllPrestamosAsync()
        {
            return await _context.Prestamos.ToListAsync();
        }

        public async Task<Prestamo?> GetPrestamoByIdAsync(int id)
        {
            return await _context.Prestamos.FindAsync(id);
        }

        public async Task<Prestamo> CreatePrestamoAsync(Prestamo prestamo)
        {
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
            return prestamo;
        }

        public async Task UpdatePrestamoAsync(Prestamo prestamo)
        {
            _context.Entry(prestamo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrestamoAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Prestamo>> SearchPrestamosByClienteAsync(string nombreCliente)
        {
            return await _context.Prestamos
                .Where(p => p.NombreCliente.Contains(nombreCliente))
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosByMangaIdAsync(int mangaId)
        {
            return await _context.Prestamos
                .Where(p => p.MangaId == mangaId)
                .ToListAsync();
        }

        public async Task<bool> PrestamoExistsAsync(int id)
        {
            return await _context.Prestamos.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Prestamos
                .Where(p => p.FechaPrestamo >= fechaInicio && p.FechaPrestamo <= fechaFin)
                .ToListAsync();
        }
    }
}