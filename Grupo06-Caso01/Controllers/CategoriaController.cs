using Grupo06_Caso01.Dtos;
using Grupo06_Caso01.Models;
using Grupo06_Caso01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grupo06_Caso01.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Listar()
    {
        var categorias = await _categoriaService.ListarAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Categoria>> ObtenerPorId(int id)
    {
        var categoria = await _categoriaService.ObtenerPorIdAsync(id);
        if (categoria is null)
            return NotFound($"No existe la categoria con id {id}.");

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> Crear(CategoriaRequestDto request)
    {
        try
        {
            var categoria = new Categoria { Nombre = request.Nombre };
            var creada = await _categoriaService.CrearAsync(categoria);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Categoriaid }, creada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, CategoriaRequestDto request)
    {
        try
        {
            var categoria = new Categoria { Nombre = request.Nombre };
            var actualizado = await _categoriaService.ActualizarAsync(id, categoria);
            if (!actualizado)
                return NotFound($"No existe la categoria con id {id}.");

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado = await _categoriaService.EliminarAsync(id);
            if (!eliminado)
                return NotFound($"No existe la categoria con id {id}.");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // La categoria tiene productos asociados y no puede eliminarse.
            return Conflict(ex.Message);
        }
    }
}
