using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ManagersApi
{
    /// <summary>
    /// Enum which represent status of project
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Flags]
    public enum ProjectStatus
    {
        [EnumMember(Value = "Undefined")] Undefined,
        [EnumMember(Value = "Active")] Active,
        [EnumMember(Value = "Inactive")] Inactive,
        
    }
}