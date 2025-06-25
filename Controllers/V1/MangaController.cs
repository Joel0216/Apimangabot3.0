using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JaveragesLibrary.Services.Features.Mangas;
using JaveragesLibrary.Domain.Entities;

namespace MiMangaBot.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // 🔒 PROTEGE TODOS LOS ENDPOINTS DE ESTE CONTROLADOR
    public class MangaController : ControllerBase
    {
        private readonly MangaService _mangaService;

        public MangaController(MangaService mangaService)
        {
            _mangaService = mangaService;
        }

        /// <summary>
        /// Obtiene todos los mangas (requiere autenticación)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manga>>> GetMangas()
        {
            try
            {
                var mangas = await _mangaService.GetAllMangasAsync();
                return Ok(new
                {
                    success = true,
                    data = mangas,
                    message = "Mangas obtenidos exitosamente",
                    user = User.Identity?.Name // Información del usuario autenticado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene un manga por ID (requiere autenticación)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Manga>> GetManga(int id)
        {
            try
            {
                var manga = await _mangaService.GetMangaByIdAsync(id);
                if (manga == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró el manga con ID {id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = manga,
                    message = "Manga encontrado exitosamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Crea un nuevo manga (requiere autenticación)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Manga>> CreateManga([FromBody] Manga manga)
        {
            try
            {
                if (manga == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Los datos del manga son requeridos"
                    });
                }

                var createdManga = await _mangaService.CreateMangaAsync(manga);
                return CreatedAtAction(
                    nameof(GetManga),
                    new { id = createdManga.Id },
                    new
                    {
                        success = true,
                        data = createdManga,
                        message = "Manga creado exitosamente",
                        createdBy = User.Identity?.Name
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al crear el manga",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Actualiza un manga existente (requiere autenticación)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManga(int id, [FromBody] Manga manga)
        {
            try
            {
                if (id != manga.Id)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El ID del manga no coincide"
                    });
                }

                var existingManga = await _mangaService.GetMangaByIdAsync(id);
                if (existingManga == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró el manga con ID {id}"
                    });
                }

                await _mangaService.UpdateMangaAsync(manga);
                return Ok(new
                {
                    success = true,
                    message = "Manga actualizado exitosamente",
                    updatedBy = User.Identity?.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al actualizar el manga",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Elimina un manga (requiere autenticación)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManga(int id)
        {
            try
            {
                var existingManga = await _mangaService.GetMangaByIdAsync(id);
                if (existingManga == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró el manga con ID {id}"
                    });
                }

                await _mangaService.DeleteMangaAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Manga eliminado exitosamente",
                    deletedBy = User.Identity?.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al eliminar el manga",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Busca mangas por título (requiere autenticación)
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Manga>>> SearchMangas([FromQuery] string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El parámetro 'titulo' es requerido para la búsqueda"
                    });
                }

                var mangas = await _mangaService.SearchMangasByTitleAsync(titulo);
                return Ok(new
                {
                    success = true,
                    data = mangas,
                    message = $"Búsqueda completada para: '{titulo}'",
                    searchedBy = User.Identity?.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error en la búsqueda",
                    error = ex.Message
                });
            }
        }
    }
}