using System.Text.Json.Serialization;

namespace Polimerida_CAP.Models;

public class FaceData
{
    [JsonPropertyName("faceLibType")]
    public string faceLibType { get; set; }

    [JsonPropertyName("FDID")]
    public string FDID { get; set; }

    [JsonPropertyName("FPID")]
    public string FPID { get; set; }

    [JsonPropertyName("featurePointType")]
    public string featurePointType { get; set; }

    [JsonPropertyName("faceURL")]
    public string faceURL { get; set; }
} 