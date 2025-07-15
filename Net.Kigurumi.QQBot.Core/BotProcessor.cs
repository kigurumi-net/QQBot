using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core
{
    public static class BotProcessor
    {
        public static string Handle(string message)
        {
            // 简单回应逻辑（可扩展）
            if (message.Contains("你好")) return "你好！我是 KigBot";
            return "收到消息";
        }
    }
}
