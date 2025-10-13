// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.ChatNotifier
{
    public partial class ChatNotifierInputManager : RulesetInputManager<ChatNotifierAction>
    {
        public ChatNotifierInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum ChatNotifierAction
    {
    }
}
