using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Empleado
{
    public uint Idempleado { get; set; }

    public string Primernombre { get; set; } = null!;

    public string Segundonombre { get; set; } = null!;

    public string Apellidopaterno { get; set; } = null!;

    public string Apellidomaterno { get; set; } = null!;

    public uint Iddepartamento { get; set; }

    public uint Idpuesto { get; set; }

    public string? Email { get; set; }

    public string Sexo { get; set; } = null!;

    public string RegStatus { get; set; } = null!;

    public DateTime Fechaingreso { get; set; }

    public int Credencial { get; set; }

    public byte[]? Image { get; set; }

    public string Nombreimagen { get; set; } = null!;

    public int Iddispositivo { get; set; }

    public DateTime Fechabaja { get; set; }

    public int? Idsubgrupo { get; set; }

    public virtual Subgrupo? IdsubgrupoNavigation { get; set; }
    public virtual Departamento? IddepartamentoNavigation { get; set; }
    public virtual Puesto? IdpuestoNavigation { get; set; }
    public virtual Dispositivo? IddispositivoNavigation { get; set; }
}
