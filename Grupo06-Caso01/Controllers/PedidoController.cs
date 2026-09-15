using Grupo06_Caso01.Models;
using Grupo06_Caso01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grupo06_Caso01.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> Listar()
    {
        var pedidos = await _pedidoService.ListarAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Pedido>> ObtenerPorId(int id)
    {
        var pedido = await _pedidoService.ObtenerPorIdAsync(id);
        if (pedido is null)
            return NotFound($"No existe el pedido con id {id}.");

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> Crear(Pedido pedido)
    {
        try
        {
            var creado = await _pedidoService.CrearAsync(pedido);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Pedidoid }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Pedido pedido)
    {
        try
        {
            var actualizado = await _pedidoService.ActualizarAsync(id, pedido);
            if (!actualizado)
                return NotFound($"No existe el pedido con id {id}.");

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
            var eliminado = await _pedidoService.EliminarAsync(id);
            if (!eliminado)
                return NotFound($"No existe el pedido con id {id}.");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}