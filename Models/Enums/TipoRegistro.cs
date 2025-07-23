using System.Text.Json.Serialization;

namespace RegistroDoPonto.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TipoRegistro
{
    [JsonPropertyName("entrada")]
    entrada,
    [JsonPropertyName("saída")]
    saída,
    [JsonPropertyName("saidaAlmoco")]
    saidaAlmoco,
    [JsonPropertyName("voltaAlmoco")]
    voltaAlmoco
}