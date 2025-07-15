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
        Task OnGroupMessageAsync(GroupMessageEvent evt);
        Task OnGroupRequestAsync(GroupRequestEvent evt);
    }
}
