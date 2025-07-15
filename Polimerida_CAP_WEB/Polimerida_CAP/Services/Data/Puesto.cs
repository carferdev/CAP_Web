using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Puesto
{
    public uint Idpuesto { get; set; }

    public string Descripcion { get; set; } = null!;

    public string RegStatus { get; set; } = null!;
}
