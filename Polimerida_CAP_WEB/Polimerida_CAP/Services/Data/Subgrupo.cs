using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Subgrupo
{
    public int Idsubgrupo { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? RegStatus { get; set; }

    public int Idgrupo { get; set; }

    public int Idturno { get; set; }

    public virtual ICollection<Empleado> Empleado { get; set; } = new List<Empleado>();
}
