// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.ChatNotifier.Beatmaps;
using osu.Game.Rulesets.ChatNotifier.Graphics;
using osu.Game.Rulesets.ChatNotifier.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Game.Database;
using osu.Game.Online.API;
using osu.Framework.Logging;
using System;

namespace osu.Game.Rulesets.ChatNotifier
{
    public partial class ChatNotifierRuleset : Ruleset
    {
        public override string Description => "Chat Notifier";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) =>
            new DrawableChatNotifierRuleset(this, beatmap, mods);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) =>
            new ChatNotifierBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) =>
            new ChatNotifierDifficultyCalculator(RulesetInfo, beatmap);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                default:
                    return [];
            }
        }

        public static readonly string SHORT_NAME = "chatnotifierruleset";
        public override string ShortName => SHORT_NAME;

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => [];

        public override Drawable CreateIcon() => new ChatNotifierIconWithListenerLoader();

        public partial class ChatNotifierIconWithListenerLoader : ChatNotifierIcon
        {
            public ChatNotifierIconWithListenerLoader()
            {
                AutoSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader(permitNulls: true)]
            private void load(OsuGame game, Storage storage, IModelImporter<BeatmapSetInfo> beatmapImporter, IAPIProvider api)
            {
                try
                {
                    Logging.Log("Begin init ListenerLoader");
                    Logging.Log($"Deps: Game = '{game}' :: Storage = '{storage}' :: Importer = '{beatmapImporter}' :: IAPIProvider = '{api}'");

                    if (!ListenerLoader.ListenerLoader.INSTANCE.BeginInject(storage, game, Scheduler))
                    {
                        Logging.Log("Injection failed!", level: LogLevel.Error);
                        return;
                    }
                }
                catch (Exception e)
                {
                    Logging.LogError(e, "Unknown exception");
                }
            }
        }

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;
    }
}
