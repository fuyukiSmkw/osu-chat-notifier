// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.ChatNotifier.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.ChatNotifier.Replays
{
    public class ChatNotifierAutoGenerator : AutoGenerator<ChatNotifierReplayFrame>
    {
        public new Beatmap<ChatNotifierHitObject> Beatmap => (Beatmap<ChatNotifierHitObject>)base.Beatmap;

        public ChatNotifierAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        protected override void GenerateFrames()
        {
            Frames.Add(new ChatNotifierReplayFrame());

            foreach (ChatNotifierHitObject hitObject in Beatmap.HitObjects)
            {
                Frames.Add(new ChatNotifierReplayFrame
                {
                    Time = hitObject.StartTime,
                    Position = hitObject.Position,
                    // todo: add required inputs and extra frames.
                });
            }
        }
    }
}
