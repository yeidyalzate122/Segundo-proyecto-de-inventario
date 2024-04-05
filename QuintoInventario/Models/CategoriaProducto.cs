using System;
using System.Collections.Generic;

namespace QuintoInventario.Models;

public partial class CategoriaProducto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
