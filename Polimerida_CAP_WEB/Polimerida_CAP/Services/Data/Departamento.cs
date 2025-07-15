using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Departamento
{
    public uint Iddepartamento { get; set; }

    public string Descripcion { get; set; } = null!;

    public string RegStatus { get; set; } = null!;
}
