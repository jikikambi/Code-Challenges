using System.Text.Json.Serialization;

namespace CodeChallenge.ApplicationLayer.Tracking.Models;

public class Event
{

    public Event(ServiceId serviceId, string operationId) : this(serviceId.ToString(), operationId)
    { }

    [JsonConstructor]
    public Event(string serviceId, string operationId)
    {
        ServiceId = serviceId;
        OperationId = operationId;
    }

    private Event()
    { }

    public string ServiceId { get; }
    public string OperationId { get; }
    public State State { get; set; } = default!;
    public List<EventAction> Incoming { get; set; } = default!;
    public List<EventAction> Outgoing { get; set; } = default!;
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}