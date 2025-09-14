using System.Text.Json.Serialization;
using pux.core;

namespace pux.core
{
    [JsonSerializable(typeof(Settings))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
