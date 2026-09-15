namespace Grupo06_Caso01.Dtos;

public class MovimientoInventarioRequestDto
{
    public int Productoid { get; set; }
    public string Tipomovimiento { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public DateTime? Fecha { get; set; }
}
