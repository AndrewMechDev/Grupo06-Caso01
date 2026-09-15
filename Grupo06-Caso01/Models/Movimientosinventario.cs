using System;
using System.Collections.Generic;

namespace Grupo06_Caso01.Models;

public partial class Movimientosinventario
{
    public int Movimientoid { get; set; }

    public int Productoid { get; set; }

    public string Tipomovimiento { get; set; } = null!;

    public int Cantidad { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
