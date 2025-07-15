using System.Text.Json.Serialization;

namespace Polimerida_CAP.Models;

public class UserInfoRequest
{
    [JsonPropertyName("UserInfo")]
    public UserInfo UserInfo { get; set; }
}

public class UserInfo
{
    [JsonPropertyName("employeeNo")]
    public string employeeNo { get; set; }

    [JsonPropertyName("name")]
    public string name { get; set; }

    [JsonPropertyName("userType")]
    public string userType { get; set; }

    [JsonPropertyName("doorRight")]
    public string doorRight { get; set; }

    [JsonPropertyName("RightPlan")]
    public RightPlan[] RightPlan { get; set; }

    [JsonPropertyName("gender")]
    public string gender { get; set; }

    [JsonPropertyName("localUIRight")]
    public bool localUIRight { get; set; }

    [JsonPropertyName("maxOpenDoorTime")]
    public int maxOpenDoorTime { get; set; }

    [JsonPropertyName("userVerifyMode")]
    public string userVerifyMode { get; set; }

    [JsonPropertyName("Valid")]
    public Valid Valid { get; set; }
}

public class RightPlan
{
    [JsonPropertyName("doorNo")]
    public int doorNo { get; set; }

    [JsonPropertyName("planTemplateNo")]
    public string planTemplateNo { get; set; }
}

public class Valid
{
    [JsonPropertyName("enable")]
    public bool enable { get; set; }

    [JsonPropertyName("beginTime")]
    public string beginTime { get; set; }

    [JsonPropertyName("endTime")]
    public string endTime { get; set; }

    [JsonPropertyName("timeType")]
    public string timeType { get; set; }
} 