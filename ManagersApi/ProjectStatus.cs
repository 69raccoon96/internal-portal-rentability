using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

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
        [EnumMember(Value = "Active")] [Description("Active")]Active,
        [EnumMember(Value = "Inactive")] [Description("Inactive")]Inactive,
        
    }
}