namespace CodeChallenge.ApplicationLayer.Tracking.Models;

public class EventAction(Direction direction)
{
    public Direction Direction { get; set; } = direction;
    public Data Data { get; set; }
    public DateTime TimeStamp { get; set; }
}