// Copyright (c) 2025 MATRIX-feather. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;

#nullable enable

namespace osu.Game.Rulesets.ChatNotifier.ListenerLoader.Handlers;

public partial class ChatNotifierRulesetIconListener : AbstractHandler
{
    [Resolved(canBeNull: true)]
    private IBindable<RulesetInfo>? ruleset { get; set; }

    [Resolved]
    private RulesetStore? rulesets { get; set; }

    [BackgroundDependencyLoader]
    private void load()
    {
        if (ruleset is not Bindable<RulesetInfo> rs) return;

        // Return to previous ruleset when changing to ChatNotifier ruleset
        ruleset.BindValueChanged(v =>
        {
            if (v.NewValue.ShortName != ChatNotifierRuleset.SHORT_NAME // Current not
            || v.OldValue.ShortName == ChatNotifierRuleset.SHORT_NAME // TriggerChange (maybe?)
            )
                return;
            if (v.OldValue == null) // No old value (start up with)
            {
                // Switch to osu!std
                var std = rulesets?.AvailableRulesets.FirstOrDefault();
                if (std is not null)
                    rs.Value = std;
                return;
            }

            // Switch back
            rs.Value = v.OldValue;
        });
    }
}
