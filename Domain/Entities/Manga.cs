using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JaveragesLibrary.Domain.Entities
{
    [Table("Mangas")]
    public class Manga
    {
        [Key]
        public int Id { get; set; }

        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public int? Capitulos { get; set; }
    }
}