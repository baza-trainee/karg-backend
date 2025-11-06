using System.Text.Json.Serialization;

namespace karg.DAL.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AnimalSortOrder
    {
        Latest,
        Oldest
    }
}
