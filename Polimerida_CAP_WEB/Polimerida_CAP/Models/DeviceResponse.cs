using System.Text.Json.Serialization;

namespace Polimerida_CAP.Models;

public class DeviceResponse
{
    public string? UserResponse { get; set; }
    public string? FaceResponse { get; set; }
    public bool UserSuccess { get; set; }
    public bool FaceSuccess { get; set; }
    public string? ErrorMessage { get; set; }
} 