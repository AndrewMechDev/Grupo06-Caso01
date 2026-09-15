using Grupo06_Caso01.Dtos;
using Grupo06_Caso01.Models;
using Grupo06_Caso01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grupo06_Caso01.Controllers;

[ApiController]
[Route("api/movimientos-inventario")]
public class MovimientoInventarioController : ControllerBase
{
    private readonly IMovimientoInventarioService _movimientoInventarioService;

    public MovimientoInventarioController(IMovimientoInventarioService movimientoInventarioService)
    {
        _movimientoInventarioService = movimientoInventarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movimientosinventario>>> Listar()
    {
        var movimientos = await _movimientoInventarioService.ListarAsync();
        return Ok(movimientos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Movimientosinventario>> ObtenerPorId(int id)
    {
        var movimiento = await _movimientoInventarioService.ObtenerPorIdAsync(id);
        if (movimiento is null)
            return NotFound($"No existe el movimiento de inventario con id {id}.");

        return Ok(movimiento);
    }

    [HttpPost]
    public async Task<ActionResult<Movimientosinventario>> Crear(MovimientoInventarioRequestDto request)
    {
        try
        {
            var movimiento = new Movimientosinventario
            {
                Productoid = request.Productoid,
                Tipomovimiento = request.Tipomovimiento,
                Cantidad = request.Cantidad,
                Fecha = request.Fecha ?? default
            };
            var creado = await _movimientoInventarioService.CrearAsync(movimiento);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Movimientoid }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            // El movimiento dejaria el stock en negativo (o superaria el limite).
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, MovimientoInventarioRequestDto request)
    {
        try
        {
            var movimiento = new Movimientosinventario
            {
                Productoid = request.Productoid,
                Tipomovimiento = request.Tipomovimiento,
                Cantidad = request.Cantidad,
                Fecha = request.Fecha ?? default
            };
            var actualizado = await _movimientoInventarioService.ActualizarAsync(id, movimiento);
            if (!actualizado)
                return NotFound($"No existe el movimiento de inventario con id {id}.");

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado = await _movimientoInventarioService.EliminarAsync(id);
            if (!eliminado)
                return NotFound($"No existe el movimiento de inventario con id {id}.");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}
