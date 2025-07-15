using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Turno
{
    public uint Idturno { get; set; }

    public DateTime Horaentrada { get; set; }

    public DateTime Horasalida { get; set; }

    public string Nombre { get; set; } = null!;

    public string RegStatus { get; set; } = null!;

    public DateTime Entradacomida { get; set; }

    public DateTime Salidacomida { get; set; }
}
