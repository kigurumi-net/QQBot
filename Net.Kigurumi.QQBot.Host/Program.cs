using Net.Kigurumi.QQBot.Connector.Client;
using Net.Kigurumi.QQBot.Connector.Server;
using Net.Kigurumi.QQBot.Host.Handlers;
using DotNetEnv;

namespace Net.Kigurumi.QQBot.Host
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 读取项目根目录下的 .env 文件
            Env.Load();

            Console.WriteLine("启动 KigBot...");

            var cts = new CancellationTokenSource();

            // === 以下为正向连接备用代码（当前未启用） ===

            // var wsClientUrl = Environment.GetEnvironmentVariable("NAPCAT_WS_CLIENT_URL") ?? "ws://localhost:3001/ws";
            // var wsClientPortStr = Environment.GetEnvironmentVariable("NAPCAT_WS_CLIENT_PORT") ?? "3001";
            // int wsClientPort = int.TryParse(wsClientPortStr, out var cPort) ? cPort : 3001;
            // if (!wsClientUrl.Contains(":"))
            // {
            //     wsClientUrl = $"ws://localhost:{wsClientPort}/ws";
            // }
            // Console.WriteLine($"WS 客户端连接地址: {wsClientUrl}");
            // var clientTask = NapcatWsClient.RunAsync(wsClientUrl, cts.Token);

            // 读取环境变量
            var wsServerUrl = Environment.GetEnvironmentVariable("NAPCAT_WS_SERVER_URL") ?? "http://localhost";
            var wsServerPortStr = Environment.GetEnvironmentVariable("NAPCAT_WS_SERVER_PORT") ?? "6001";

            // 端口转换
            int wsServerPort = int.TryParse(wsServerPortStr, out var sPort) ? sPort : 6001;

            // 组合服务端完整 URL（主要给 HttpListener 使用，通常是 http 协议）
            var wsServerPrefix = $"{wsServerUrl}:{wsServerPort}/ws/";

            Console.WriteLine($"WS 服务端监听地址: {wsServerPrefix}");

            // 启动 WS 服务端
            var handler = new BotHandler();
            var server = new NapcatWsServer(wsServerPrefix,handler);
            var serverTask = server.StartAsync(cts.Token);

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
            cts.Cancel();
            await serverTask; // 只运行服务端
        }
    }
}
