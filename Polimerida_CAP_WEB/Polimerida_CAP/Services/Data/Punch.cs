using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Punch
{
    public uint Idpunches { get; set; }

    public DateTime Regtimestamp { get; set; }

    public DateTime Fechapunch { get; set; }

    public string Softkey { get; set; } = null!;

    public uint Idempleado { get; set; }

    public string Nombredispositivo { get; set; } = null!;

    public uint Idgrupo { get; set; }

    public uint Idturno { get; set; }

    public uint Penalizacion { get; set; }

    public int Iddispositivo { get; set; }
}
