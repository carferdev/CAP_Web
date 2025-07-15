using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Incidenciaxempleado
{
    public uint Idincidenciaxempleado { get; set; }

    public uint Idempleado { get; set; }

    public uint Idincidencia { get; set; }

    public DateTime Fechainicio { get; set; }

    public DateTime Fechafin { get; set; }

    public string RegStatus { get; set; } = null!;

    public virtual Empleado IdempleadoNavigation { get; set; } = null!;
    public virtual Incidencias IdincidenciaNavigation { get; set; } = null!;
}
