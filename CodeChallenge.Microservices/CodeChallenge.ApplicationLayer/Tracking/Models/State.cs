namespace CodeChallenge.ApplicationLayer.Tracking.Models;

public class State(StateType type, string? message = default)
{
    /// <summary>
    /// Current state of the tracked flow
    /// Can either be: Pending (default), TechnicalError, FunctionalError, Processed
    /// </summary>
    public StateType Type { get; set; } = type;
    public string? Message { get; set; } = message;
    public DateTime ProcessedOn { get; set; } = DateTime.UtcNow;
}