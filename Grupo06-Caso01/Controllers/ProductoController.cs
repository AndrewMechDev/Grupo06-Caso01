using Grupo06_Caso01.Models;
using Grupo06_Caso01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grupo06_Caso01.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> Listar()
    {
        var productos = await _productoService.ListarAsync();
        return Ok(productos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Producto>> ObtenerPorId(int id)
    {
        var producto = await _productoService.ObtenerPorIdAsync(id);
        if (producto is null)
            return NotFound($"No existe el producto con id {id}.");

        return Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<Producto>> Crear(Producto producto)
    {
        try
        {
            var creado = await _productoService.CrearAsync(producto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Productoid }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Producto producto)
    {
        try
        {
            var actualizado = await _productoService.ActualizarAsync(id, producto);
            if (!actualizado)
                return NotFound($"No existe el producto con id {id}.");

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
            var eliminado = await _productoService.EliminarAsync(id);
            if (!eliminado)
                return NotFound($"No existe el producto con id {id}.");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // El producto tiene movimientos de inventario asociados y no puede eliminarse.
            return Conflict(ex.Message);
        }
    }
}