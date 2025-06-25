using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JaveragesLibrary.Services.Features.Prestamos;
using JaveragesLibrary.Domain.Entities;

namespace MiMangaBot.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] // 🔒 PROTEGE TODOS LOS ENDPOINTS DE ESTE CONTROLADOR
    public class PrestamoController : ControllerBase
    {
        private readonly PrestamoService _prestamoService;

        public PrestamoController(PrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        /// <summary>
        /// Obtiene todos los préstamos (requiere autenticación)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos()
        {
            try
            {
                var prestamos = await _prestamoService.GetAllPrestamosAsync();
                return Ok(new
                {
                    success = true,
                    data = prestamos,
                    message = "Préstamos obtenidos exitosamente",
                    user = User.Identity?.Name
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
        /// Obtiene un préstamo por ID (requiere autenticación)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            try
            {
                var prestamo = await _prestamoService.GetPrestamoByIdAsync(id);
                if (prestamo == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró el préstamo con ID {id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = prestamo,
                    message = "Préstamo encontrado exitosamente"
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
        /// Crea un nuevo préstamo (requiere autenticación)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Prestamo>> CreatePrestamo([FromBody] Prestamo prestamo)
        {
            try
            {
                if (prestamo == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Los datos del préstamo son requeridos"
                    });
                }

                // Establecer la fecha actual si no se proporciona
                if (prestamo.FechaPrestamo == default)
                {
                    prestamo.FechaPrestamo = DateTime.Now;
                }

                var createdPrestamo = await _prestamoService.CreatePrestamoAsync(prestamo);
                return CreatedAtAction(
                    nameof(GetPrestamo),
                    new { id = createdPrestamo.Id },
                    new
                    {
                        success = true,
                        data = createdPrestamo,
                        message = "Préstamo creado exitosamente",
                        createdBy = User.Identity?.Name
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al crear el préstamo",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Actualiza un préstamo existente (requiere autenticación)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrestamo(int id, [FromBody] Prestamo prestamo)
        {
            try
            {
                if (id != prestamo.Id)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El ID del préstamo no coincide"
                    });
                }

                var existingPrestamo = await _prestamoService.GetPrestamoByIdAsync(id);
                if (existingPrestamo == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró el préstamo con ID {id}"
                    });
                }

                await _prestamoService.UpdatePrestamoAsync(prestamo);
                return Ok(new
                {
                    success = true,
                    message = "Préstamo actualizado exitosamente",
                    updatedBy = User.Identity?.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al actualizar el préstamo",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Elimina un préstamo (requiere autenticación)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            try
            {
                var existingPrestamo = await _prestamoService.GetPrestamoByIdAsync(id);
                if (existingPrestamo == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró el préstamo con ID {id}"
                    });
                }

                await _prestamoService.DeletePrestamoAsync(id);
                return Ok(new
                {
                    success = true,
                    message = "Préstamo eliminado exitosamente",
                    deletedBy = User.Identity?.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al eliminar el préstamo",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Busca préstamos por nombre del cliente (requiere autenticación)
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> SearchPrestamos([FromQuery] string cliente)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cliente))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El parámetro 'cliente' es requerido para la búsqueda"
                    });
                }

                var prestamos = await _prestamoService.SearchPrestamosByClienteAsync(cliente);
                return Ok(new
                {
                    success = true,
                    data = prestamos,
                    message = $"Búsqueda completada para cliente: '{cliente}'",
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

        /// <summary>
        /// Obtiene préstamos por ID de manga (requiere autenticación)
        /// </summary>
        [HttpGet("manga/{mangaId}")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosByManga(int mangaId)
        {
            try
            {
                var prestamos = await _prestamoService.GetPrestamosByMangaIdAsync(mangaId);
                return Ok(new
                {
                    success = true,
                    data = prestamos,
                    message = $"Préstamos del manga ID {mangaId} obtenidos exitosamente",
                    user = User.Identity?.Name
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
    }
}