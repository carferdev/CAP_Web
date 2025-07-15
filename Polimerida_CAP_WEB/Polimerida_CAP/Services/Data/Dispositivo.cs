using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Dispositivo
{
    public int Iddispositivo { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Clase { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string RegStatus { get; set; } = null!;
}
