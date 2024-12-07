namespace Example06.Extensions;

public static partial class LoggingExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Message ({MessageId}) consumed.")]
    public static partial void LogConsumedMessage(this ILogger logger, Guid messageId);

    [LoggerMessage(Level = LogLevel.Information, Message = "Message ({MessageId}) produced.")]
    public static partial void LogProducedMessage(this ILogger logger, Guid messageId);
}