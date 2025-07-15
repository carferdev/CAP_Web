using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Incidencia
{
    public uint Idincidencia { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string RegStatus { get; set; } = null!;
}
