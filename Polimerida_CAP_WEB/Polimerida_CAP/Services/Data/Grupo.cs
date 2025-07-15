using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Grupo
{
    public uint Idgrupo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string RegStatus { get; set; } = null!;
}
