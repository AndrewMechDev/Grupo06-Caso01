using System;
using System.Collections.Generic;

namespace Grupo06_Caso01.Models;

public partial class Pedido
{
    public int Pedidoid { get; set; }

    public int Proveedorid { get; set; }

    public DateTime Fechapedido { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Proveedore Proveedor { get; set; } = null!;
}
