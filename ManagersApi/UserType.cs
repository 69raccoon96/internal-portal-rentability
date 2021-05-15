using System;
using System.Text.Json.Serialization;

namespace ManagersApi
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Flags]
    public enum UserType
    {
        Manager,
        Leader
    }
}