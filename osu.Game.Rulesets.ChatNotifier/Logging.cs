// Copyright (c) 2025 MATRIX-feather. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Logging;

#nullable enable

namespace osu.Game.Rulesets.ChatNotifier;

public class Logging
{
    public static readonly string LOG_PREFIX = "ChatNotifier";

    public static void Log(string? message, LoggingTarget loggingTarget = LoggingTarget.Runtime, LogLevel level = LogLevel.Verbose)
    {
        Logger.Log($"[{LOG_PREFIX}] {message}", level: level, target: loggingTarget);
    }

    public static void LogError(Exception e, string? message = null)
    {
        while (true)
        {
            Logger.Log($"[{LOG_PREFIX}] {(string.IsNullOrEmpty(message) ? "" : $"{message}: ")}{e.Message}", level: LogLevel.Important);
            Logger.Log(e.StackTrace);

            if (e.InnerException != null)
            {
                e = e.InnerException;
                message = null;
                continue;
            }

            break;
        }
    }
}
