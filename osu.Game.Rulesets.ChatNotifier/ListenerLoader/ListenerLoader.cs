// Copyright (c) 2025 MATRIX-feather. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osu.Game.Rulesets.ChatNotifier.ListenerLoader.Handlers;

#nullable enable

namespace osu.Game.Rulesets.ChatNotifier.ListenerLoader;

public partial class ListenerLoader : AbstractHandler
{
    public static readonly ListenerLoader INSTANCE = new();

    /// <summary>
    /// HashCode of injected OsuGame; -1 indicates that the game hasn't been injected
    /// </summary>
    private static int currentSessionHash = -2;

    public static int GetRegisteredSessionHash()
    {
        return currentSessionHash;
    }

    private static AbstractHandler[] injectors()
    {
        return
        [
            new ChatNotifierRulesetIconListener(),
            new NewChatNotificationListener(),
        ];
    }

    public bool BeginInject(Storage storage, OsuGame? gameInstance, Scheduler scheduler)
    {
        int sessionHashCode = gameInstance?.Toolbar.GetHashCode() ?? -1;

        if (currentSessionHash == sessionHashCode)
        {
            Logging.Log("Duplicate dependency inject call for current session, skipping...");
            return true;
        }

        currentSessionHash = sessionHashCode;

        if (gameInstance?.Dependencies is not DependencyContainer depMgr)
        {
            Logging.Log($"DependencyContainer not found", level: LogLevel.Error);
            return false;
        }

        try
        {
            // Add Resource store
            gameInstance.Resources.AddStore(new DllResourceStore(typeof(ChatNotifierRuleset).Assembly));

            scheduler.AddDelayed(() =>
            {
                gameInstance.AddRange(injectors());
            }, 1);
        }
        catch (Exception e)
        {
            Logging.LogError(e, "Error injecting game instance: ");
            Logging.Log(e.Message, level: LogLevel.Important);
            return false;
        }

        Logging.Log("Initial inject done!");

        return true;
    }
}
