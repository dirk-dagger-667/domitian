﻿using Microsoft.Extensions.Logging;

namespace domitian.Infrastructure.Logging
{
    public static partial class Log
    {
        [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Critical,
        Message = "Could not open socket to `{HostName}`")]
        public static partial void CouldNotOpenSocket(
        this ILogger logger, string hostName);
    }
}
