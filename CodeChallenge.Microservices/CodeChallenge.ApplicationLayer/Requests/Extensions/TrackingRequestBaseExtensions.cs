using CodeChallenge.ApplicationLayer.Requests.Services;
using CodeChallenge.ApplicationLayer.Tracking.Models;
using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Requests.Extensions;

[ExcludeFromCodeCoverage(Justification = "Will be tested in a later")]
public static class TrackingRequestBaseExtensions
{

    public static void SetPending<TInput>(this ITrackingRequestBase<TInput> request) => SetState(request, StateType.Pending);
    public static void SetProcessed<TInput>(this ITrackingRequestBase<TInput> request) => SetState(request, StateType.Processed);
    public static void SetTechnicalError<TInput>(this ITrackingRequestBase<TInput> request, Exception ex) => SetState(request, StateType.TechnicalError, ex.Message);
    public static void SetFunctionalError<TInput>(this ITrackingRequestBase<TInput> request, Exception ex) => SetState(request, StateType.FunctionalError, ex.Message);

    public static void SetMessage<TInput>(this ITrackingRequestBase<TInput> request, string message)
    {
        if (request.Event.State == default)
        {
            SetState(request, StateType.Pending, message);
        }
        else
        {
            request.Event.State.Message = message;
        }
    }

    public static void AddInComingData<T1Input, T2Input>(this ITrackingRequestBase<T1Input> request, T2Input entity, string? message = default)
    {
        var action = CreateEventAction(entity, Direction.Incoming, message);

        if (request.Event.Incoming == default)
        {
            request.Event.Incoming = [];
        }
        request.Event.Incoming.Add(action);
    }

    public static void AddOutGoingData<TInput, TOutput>(this ITrackingRequestBase<TInput> request, TOutput entity, string? message = default)
    {
        var action = CreateEventAction(entity, Direction.Outgoing, message);

        if (request.Event.Outgoing == default)
        {
            request.Event.Outgoing = [];
        }
        request.Event.Outgoing.Add(action);
    }

    private static void SetState<TInput>(ITrackingRequestBase<TInput> request, StateType type, string? message = default)
    {
        if (request.Event == default) throw new InvalidOperationException();

        if (request.Event.State != default)
        {
            request.Event.State.Type = type;

            if (!string.IsNullOrEmpty(message))
            {
                request.Event.State.Message += message;
            }
        }
        else
        {
            request.Event.State = new State(type, message);
        }
    }

    private static EventAction CreateEventAction<TInput>(TInput entity, Direction direction, string? message = default)
    {
        var action = new EventAction(direction)
        {
            Data = new Data
            {
                Message = message
            }
        };
        action.Data.Set(entity);
        return action;
    }
}