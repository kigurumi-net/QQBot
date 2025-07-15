using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.Kigurumi.QQBot.Core.Events;
using Net.Kigurumi.QQBot.Core.Interfaces;

namespace Net.Kigurumi.QQBot.Host.Handlers
{
    public class BotHandler : IOneBotEventHandler
    {
        public Task OnGroupMessageAsync(GroupMessageEvent evt)
        {
            Console.WriteLine($"[群消息] 群: {evt.GroupId}, 用户: {evt.UserId}, 内容: {evt.Message}");
            return Task.CompletedTask;
        }

        public Task OnGroupRequestAsync(GroupRequestEvent evt)
        {
            Console.WriteLine($"[加群申请] 用户: {evt.UserId}, 备注: {evt.Comment}, 群: {evt.GroupId}");
            return Task.CompletedTask;
        }
    }
}
