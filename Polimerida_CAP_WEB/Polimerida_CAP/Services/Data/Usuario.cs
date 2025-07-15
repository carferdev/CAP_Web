using System;
using System.Collections.Generic;

namespace Polimerida_CAP.Services.Data;

public partial class Usuario
{
    public uint Idusuario { get; set; }

    public string? Usuario1 { get; set; }

    public string? Password { get; set; }

    public string RegStatus { get; set; } = null!;
}
