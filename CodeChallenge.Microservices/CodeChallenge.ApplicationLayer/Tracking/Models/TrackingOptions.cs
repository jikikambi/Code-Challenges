namespace CodeChallenge.ApplicationLayer.Tracking.Models;

public sealed class TrackingOptions
{
    public string ElasticsearchUrl { get; set; } = default!;
    public string IndexPrefix { get; set; } = default!;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool AutoRegisterTemplate { get; set; } = true;
    public string? IngestPipeline { get; set; } // if you use an ES ingest pipeline
}