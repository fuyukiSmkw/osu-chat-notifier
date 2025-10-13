// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Online.API;
using osu.Game.Rulesets.ChatNotifier.Online.Chat;

namespace osu.Game.Rulesets.ChatNotifier.Online.API.Requests;

// https://osu.ppy.sh/docs/#get-apiv2chatpresence
public class GetChatPresenceRequest : APIRequest<List<ChatPresence>>
{
    public GetChatPresenceRequest() { }

    protected override string Target => $@"chat/presence";
}
