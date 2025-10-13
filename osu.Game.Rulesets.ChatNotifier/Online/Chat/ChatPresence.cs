// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

#nullable enable

using Newtonsoft.Json;
using osu.Game.Online.Chat;

namespace osu.Game.Rulesets.ChatNotifier.Online.Chat;

public class ChatPresence
{
    [JsonProperty(@"channel_id")]
    public long ChannelId;

    [JsonProperty(@"description")]
    public string? Description;

    [JsonProperty(@"icon")]
    public string? IconUrl;

    [JsonProperty(@"message_length_limit")]
    public long? MessageLengthLimit;

    [JsonProperty(@"moderated")]
    public bool? Moderated;

    [JsonProperty(@"name")]
    public string? Name;

    [JsonProperty(@"type")]
    public ChannelType type;

    // [JsonProperty(@"uuid")] // null only?

    // [JsonProperty(@"current_user_attributes")] // not needed

    [JsonProperty(@"last_message_id")]
    public long? LastMessageId;

    [JsonProperty(@"last_read_id")]
    public long? lastReadId;

    // [JsonProperty(@"users")] // not needed
}
