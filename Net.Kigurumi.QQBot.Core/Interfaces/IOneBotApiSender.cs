using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core.Interfaces
{
    public interface IOneBotApiSender
    {
        Task SendAsync<T>(T apiRequest);
    }
}
