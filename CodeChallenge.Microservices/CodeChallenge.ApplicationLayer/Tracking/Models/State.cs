namespace CodeChallenge.ApplicationLayer.Tracking.Models;

public class State(StateType type, string? message = default)
{
    public StateType Type { get; set; } = type;
    public string? Message { get; set; } = message;
    public DateTime ProcessedOn { get; set; } = DateTime.UtcNow;
}