// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.ChatNotifier.Objects;
using osu.Game.Rulesets.ChatNotifier.Objects.Drawables;
using osu.Game.Rulesets.ChatNotifier.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.ChatNotifier.UI
{
    [Cached]
    public partial class DrawableChatNotifierRuleset : DrawableRuleset<ChatNotifierHitObject>
    {
        public DrawableChatNotifierRuleset(ChatNotifierRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override Playfield CreatePlayfield() => new ChatNotifierPlayfield();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new ChatNotifierFramedReplayInputHandler(replay);

        public override DrawableHitObject<ChatNotifierHitObject> CreateDrawableRepresentation(ChatNotifierHitObject h) => new DrawableChatNotifierHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new ChatNotifierInputManager(Ruleset?.RulesetInfo);
    }
}
