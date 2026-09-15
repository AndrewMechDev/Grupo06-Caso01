using System;
using System.Collections.Generic;

namespace Grupo06_Caso01.Models;

public partial class Proveedore
{
    public int Proveedorid { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Contacto { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
