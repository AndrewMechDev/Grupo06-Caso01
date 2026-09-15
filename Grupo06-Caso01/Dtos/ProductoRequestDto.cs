namespace Grupo06_Caso01.Dtos;

public class ProductoRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stockactual { get; set; }
    public int Stockminimo { get; set; }
    public int Categoriaid { get; set; }
}
