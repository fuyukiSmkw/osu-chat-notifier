// Copyright (c) 2025 fuyukiS <fuyukiS@outlook.jp>. Licensed under the MIT License.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Threading;
using osu.Game.Online.API;
using osu.Game.Online.API.Requests;
using osu.Game.Online.Chat;
using osu.Game.Overlays;
using osu.Game.Rulesets.ChatNotifier.Online.API.Requests;
using osu.Game.Rulesets.ChatNotifier.Online.Chat;
using static osu.Game.Online.Chat.MessageNotifier;

#nullable enable

namespace osu.Game.Rulesets.ChatNotifier.ListenerLoader.Handlers;

public partial class NewChatNotificationListener : AbstractHandler
{
    private readonly double time_between_retries = 5000.0; // ms
    private readonly int max_retry_count = 5;

    [Resolved]
    private IAPIProvider api { get; set; } = null!;

    [Resolved]
    private INotificationOverlay? notificationOverlay { get; set; } = null!;

    #region getChatPresence

    private bool hasResult = false;
    private GetChatPresenceRequest lastReq = null!;
    private double? lastTimeReq;
    private Task<bool> requestChatPresence()
    {
        lastReq?.Cancel();
        var tcs = new TaskCompletionSource<bool>();
        var req = new GetChatPresenceRequest();

        req.Success += result =>
        {
            chatPresenceReceived(result
                .Where(r => r.LastReadId.HasValue && r.LastMessageId > r.LastReadId) // filter to unread channels only
                .Where(r => r.Type != ChannelType.Public) // filter public channels
                );
            tcs.SetResult(true);
        };
        req.Failure += _ => tcs.SetResult(false);

        api.Queue(req);
        lastReq = req;
        return tcs.Task;
    }

    private ScheduledDelegate? scheduledReq;
    private bool reqActive = false;
    private int reqCount = 0;
    private void doReq()
    {
        Debug.Assert(ThreadSafety.IsUpdateThread);
        scheduledReq = null;
        reqActive = true;
        if (++reqCount > max_retry_count)
        {
            Logging.Log($"Tried and failed {max_retry_count} times to fetch chat presence, give up.", level: Framework.Logging.LogLevel.Important);
            return;
        }
        requestChatPresence().ContinueWith(_ => reqComplete());
    }

    private void reqComplete()
    {
        lastTimeReq = Time.Current;
        reqActive = false;
        if (scheduledReq == null)
            reqIfNecessary();
    }

    private void reqIfNecessary()
    {
        if (!IsLoaded) return;
        if (reqActive) return;
        if (hasResult) return; // dont have to req anymore
        if (time_between_retries == 0) return;
        if (!lastTimeReq.HasValue)
        {
            Scheduler.AddOnce(doReq);
            return;
        }
        if (Time.Current - lastTimeReq.Value > time_between_retries)
        {
            Scheduler.AddOnce(doReq);
            return;
        }
        // not enough time
        scheduleNextReq();
    }
    private void scheduleNextReq()
    {
        scheduledReq?.Cancel();
        double lastReqDuration = lastTimeReq.HasValue ? Time.Current - lastTimeReq.Value : 0;
        scheduledReq = Scheduler.AddDelayed(doReq, Math.Max(0, time_between_retries - lastReqDuration));
    }

    #endregion getChatPresence

    #region sendNotification

    private void chatPresenceReceived(IEnumerable<ChatPresence> presentChannels)
    {
        hasResult = true;
        foreach (var c in presentChannels)
        {
            fetchChannelMessages(c);
        }
    }

    private void fetchChannelMessages(ChatPresence p, int tryCount = 1)
    {
        if (tryCount > max_retry_count)
        {
            Logging.Log($"Tried and failed {max_retry_count} times to fetch channel name {p.Name}, give up.", level: Framework.Logging.LogLevel.Important);
            return;
        }

        var req = new GetMessagesRequest(p.ToChannel());
        req.Success += sendNotification;
        req.Failure += _ => Scheduler.AddDelayed(() => fetchChannelMessages(p, tryCount + 1), time_between_retries);
        api.Queue(req);
    }

    private void sendNotification(List<Message> messages)
    {
        if (messages.Count > 0)
        {
            var message = messages[messages.Count - 1]; // filter to only the last message so that only 1 noti is sent for 1 channel
            notificationOverlay?.Post(new PrivateMessageNotification(message, new Channel
            {
                Id = message.ChannelId,
            }));
        }
    }

    #endregion sendNotification

    protected override void LoadComplete()
    {
        base.LoadComplete();
        reqIfNecessary();
    }
}
