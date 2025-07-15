using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polimerida_CAP.Services.Data;

[Table("device_registration_logs")]
public class DeviceRegistrationLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empleado")]
    public int IdEmpleado { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; }

    [Column("user_response")]
    public string? UserResponse { get; set; }

    [Column("face_response")]
    public string? FaceResponse { get; set; }

    [Column("user_success")]
    public bool UserSuccess { get; set; }

    [Column("face_success")]
    public bool FaceSuccess { get; set; }

    [Column("error_message")]
    public string? ErrorMessage { get; set; }

    [ForeignKey("IdEmpleado")]
    public virtual Empleado? Empleado { get; set; }
} 