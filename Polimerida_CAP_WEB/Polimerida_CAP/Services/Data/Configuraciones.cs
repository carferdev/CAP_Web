using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Configuraciones
{
    public uint Idconfiguraciones { get; set; }

    public string Valor { get; set; } = null!;

    public string Descripcion { get; set; } = null!;
}
