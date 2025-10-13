// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.ChatNotifier.Graphics;

public partial class ChatNotifierIcon : CompositeDrawable
{
    public ChatNotifierIcon()
    {
        InternalChildren =
        [
            new SpriteIcon
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(20),
                Icon = FontAwesome.Regular.Circle,
                Colour = Color4.White,
            },
            new SpriteIcon
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(10),
                Icon = FontAwesome.Solid.Comments,
                Colour = Color4.White,
            },
        ];
    }
}
