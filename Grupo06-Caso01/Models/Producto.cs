using System;
using System.Collections.Generic;

namespace Grupo06_Caso01.Models;

public partial class Producto
{
    public int Productoid { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stockactual { get; set; }

    public int Stockminimo { get; set; }

    public int Categoriaid { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Movimientosinventario> Movimientosinventarios { get; set; } = new List<Movimientosinventario>();
}
