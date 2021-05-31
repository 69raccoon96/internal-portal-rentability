using System;
using System.Text.Json.Serialization;

namespace ManagersApi
{
    /// <summary>
    /// Enum which represent user types
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Flags]
    public enum UserType
    {
        Manager,
        Leader
    }
}