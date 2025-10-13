// Copyright (c) 2025 MATRIX-feather. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;

namespace osu.Game.Rulesets.ChatNotifier.ListenerLoader.Utils;

public static class DependencyContainerExtension
{
    public static object? getFromCache<T>(this DependencyContainer container)
        where T : class
    {
        var cached = container.FindInstance("cache") as Dictionary<CacheInfo, object> ?? null!;
        return cached.FirstOrDefault(c => (c.Key.FindInstance("Type") as Type)! == typeof(T)).Value;
    }

    // Copyright (c) cdwcgt cdwcgt@cdwcgt.top>. Licensed under the MIT License.
    public static void replaceOrCacheAs<T>(this DependencyContainer container, T replacement)
        where T : class
    {
        var cached = container.FindInstance("cache") as Dictionary<CacheInfo, object> ?? null!;

        var cacheInfo = cached.FirstOrDefault(c => (c.Key.FindInstance("Type") as Type)! == typeof(T)).Key;

        if (cacheInfo.Equals(new CacheInfo()))
        {
            container.CacheAs<T>(replacement);
            return;
        }

        cached[cacheInfo] = replacement;
    }
}
