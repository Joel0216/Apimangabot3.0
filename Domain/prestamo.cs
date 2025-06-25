using System;

namespace JaveragesLibrary.Domain.Entities

{
    public class Prestamo
    {
        public int Id { get; set; }
        public int MangaId { get; set; }
        public string NombreCliente { get; set; } = null!;
        public DateTime FechaPrestamo { get; set; }
    }
}