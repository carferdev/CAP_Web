using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Festivo
{
    public uint Idfestivo { get; set; }

    public DateTime Fecha { get; set; }

    public string Descripcion { get; set; } = null!;

    public string RegStatus { get; set; } = null!;
}
