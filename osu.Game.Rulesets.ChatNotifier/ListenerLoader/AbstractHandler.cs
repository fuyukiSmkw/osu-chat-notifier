// Copyright (c) 2025 MATRIX-feather. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.ChatNotifier.ListenerLoader.Utils;

namespace osu.Game.Rulesets.ChatNotifier.ListenerLoader;

#nullable enable

public abstract partial class AbstractHandler : CompositeDrawable
{
    [Resolved]
    private OsuGame game { get; set; } = null!;

    protected OsuGame Game => game;

    protected static readonly BindingFlags INSTANCE_OR_STATIC_FLAG = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;

    protected static FieldInfo? FindFieldInstance(object obj, Type type)
    {
        return obj.FindFieldInstance(type);
    }
}
