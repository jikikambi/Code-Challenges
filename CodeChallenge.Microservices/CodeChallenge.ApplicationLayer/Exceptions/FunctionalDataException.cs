using System.Diagnostics.CodeAnalysis;

namespace CodeChallenge.ApplicationLayer.Exceptions;

/// <summary>
/// Exception thrown when the data in a request is invalid.
/// Incase one is not in control of the data, so one cannot correct it.
/// If the exception is thrown, the request will be marked as unprocessed and no further action will be taken.
/// </summary>
[ExcludeFromCodeCoverage(Justification = JustificationReason.NoLogic)]
public class FunctionalDataException : Exception
{
    public FunctionalDataException(string message, Exception innerException) : base(message, innerException)
    {}

    public FunctionalDataException(string message) : base(message)
    {}
}