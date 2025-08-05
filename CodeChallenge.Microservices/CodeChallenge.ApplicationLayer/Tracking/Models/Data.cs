using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Tracking.Models;

[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public class Data
{
    public void Set<TEntity>(TEntity entity)
    {
        Type = typeof(TEntity).FullName;
        Json = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        });
    }

    public TEntity? GetJsonObject<TEntity>() => Json != null
        ? JsonConvert.DeserializeObject<TEntity>(Json)
        : default;

    public string? Type { get; set; }
    public string? Json { get; set; }
    public string? Message { get; set; }
}
