namespace Grupo06_Caso01.Dtos;

public class PedidoRequestDto
{
    public int Proveedorid { get; set; }
    public DateTime? Fechapedido { get; set; }
    public string? Estado { get; set; }
}
