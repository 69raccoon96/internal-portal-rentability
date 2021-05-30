using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ManagersApi
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Flags]
    public enum ProjectStatus
    {
        [EnumMember(Value = "Undefined")] Undefined,
        [EnumMember(Value = "Active")] Active,
        [EnumMember(Value = "Inactive")] Inactive,
        
    }
}