using Grupo06_Caso01.Dtos;
using Grupo06_Caso01.Models;
using Grupo06_Caso01.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grupo06_Caso01.Controllers;

[ApiController]
[Route("api/proveedores")]
public class ProveedorController : ControllerBase
{
    private readonly IProveedorService _proveedorService;

    public ProveedorController(IProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Proveedore>>> Listar()
    {
        var proveedores = await _proveedorService.ListarAsync();
        return Ok(proveedores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Proveedore>> ObtenerPorId(int id)
    {
        var proveedor = await _proveedorService.ObtenerPorIdAsync(id);
        if (proveedor is null)
            return NotFound($"No existe el proveedor con id {id}.");

        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<ActionResult<Proveedore>> Crear(ProveedorRequestDto request)
    {
        try
        {
            var proveedor = new Proveedore
            {
                Nombre = request.Nombre,
                Contacto = request.Contacto,
                Telefono = request.Telefono,
                Email = request.Email
            };
            var creado = await _proveedorService.CrearAsync(proveedor);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Proveedorid }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, ProveedorRequestDto request)
    {
        try
        {
            var proveedor = new Proveedore
            {
                Nombre = request.Nombre,
                Contacto = request.Contacto,
                Telefono = request.Telefono,
                Email = request.Email
            };
            var actualizado = await _proveedorService.ActualizarAsync(id, proveedor);
            if (!actualizado)
                return NotFound($"No existe el proveedor con id {id}.");

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
            var eliminado = await _proveedorService.EliminarAsync(id);
            if (!eliminado)
                return NotFound($"No existe el proveedor con id {id}.");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // El proveedor tiene pedidos asociados y no puede eliminarse.
            return Conflict(ex.Message);
        }
    }
}
