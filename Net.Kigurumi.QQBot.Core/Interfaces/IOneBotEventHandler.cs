using Net.Kigurumi.QQBot.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core.Interfaces
{
    public interface IOneBotEventHandler
    {
        // 群
        Task OnGroupMessageAsync(GroupMessageEvent evt);
        
        Task OnGroupRequestAsync(GroupRequestEvent evt);
        
        // 群成员增加通知
        Task OnGroupIncreaseAsync(GroupIncreaseNoticeEvent evt);
        
        // 群成员减少
        Task OnGroupDecreaseAsync(GroupDecreaseNoticeEvent evt);
        
        // 发送消息
        
        // 发送群消息
        Task OnGroupMessageSentAsync(GroupMessageEvent evt);
        
        //发送私聊消息
        Task OnPrivateMessageSentAsync(PrivateMessageEvent evt);
        
        // 私聊
        Task OnPrivateMessageAsync(PrivateMessageEvent evt);
        
        // 元事件，如心跳
        Task OnMetaEventAsync(MetaEvent evt);
    }
}
