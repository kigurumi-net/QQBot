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
            Console.WriteLine($"[群消息] 群: {evt.GroupId}, 用户: {evt.UserId}, 内容: {evt.ParsedMessage}");
            return Task.CompletedTask;
        }

        public Task OnGroupRequestAsync(GroupRequestEvent evt)
        {
            Console.WriteLine($"[加群申请] 用户: {evt.UserId}, 备注: {evt.Comment}, 群: {evt.GroupId}");
            return Task.CompletedTask;
        }

        public Task OnPrivateMessageAsync(PrivateMessageEvent evt)
        {
            Console.WriteLine($"[私聊消息] 发送人: {evt.UserId} , 内容: {evt.Message}");
            return Task.CompletedTask;
        }

        public Task OnGroupIncreaseAsync(GroupIncreaseNoticeEvent evt)
        {
            //Console.WriteLine($"[群成员增加] 群号: {evt.GroupId}，新成员: {evt.UserId}，操作人: {evt.OperatorId}，子类型: {evt.SubType}");
            
            var action = evt.SubType switch
            {
                "invite" => "邀请加入",
                "approve" => "同意加群",
                _ => "未知操作"
            };
            Console.WriteLine($"[群成员增加] 群号: {evt.GroupId}，成员: {evt.UserId}（由 {evt.OperatorId} {action}）");
            
            return Task.CompletedTask;
        }


        public Task OnMetaEventAsync(MetaEvent evt)
        {
            if (evt.MetaEventType == "heartbeat")
            {
                Console.WriteLine($"[心跳] 间隔: {evt.Interval} 秒，Bot ID: {evt.SelfId}");
            }
            else
            {
                Console.WriteLine($"[元事件] 类型: {evt.MetaEventType}，时间: {DateTimeOffset.FromUnixTimeSeconds(evt.Time)}，Bot ID: {evt.SelfId}");
            }
            return Task.CompletedTask;
        }

        public Task OnGroupDecreaseAsync(GroupDecreaseNoticeEvent evt)
        {
            var action = evt.SubType switch
            {
                "leave" => "主动退群",
                "kick" => "被踢出",
                "kick_me" => "机器人被踢出",
                _ => "未知操作"
            };
            Console.WriteLine($"[{DateTimeOffset.FromUnixTimeSeconds(evt.Time)}][群成员减少] 群号: {evt.GroupId}，成员: {evt.UserId}，操作者: {evt.OperatorId}，行为: {action}");
            return Task.CompletedTask;
        }

        public Task OnPrivateMessageSentAsync(PrivateMessageEvent evt)
        {
            Console.WriteLine($"[私聊消息]自己给：{evt.UserId} 发消息：{evt.ParsedMessage}");
            return Task.CompletedTask;
        }

        public Task OnGroupMessageSentAsync(GroupMessageEvent evt)
        {
            Console.WriteLine($"[群消息]自己在群：{evt.GroupId} 发消息：{evt.ParsedMessage}");
            return Task.CompletedTask;
        }
        
    }
}
