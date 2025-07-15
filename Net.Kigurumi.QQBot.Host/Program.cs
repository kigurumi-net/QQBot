using Net.Kigurumi.QQBot.Connector.Client;
using Net.Kigurumi.QQBot.Connector.Server;

namespace Net.Kigurumi.QQBot.Host
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("启动 KigBot...");

            var cts = new CancellationTokenSource();

            // 启动 WS 客户端
            var clientTask = NapcatWsClient.RunAsync("ws://localhost:3000/ws", cts.Token);

            // 启动 WS 服务端
            var server = new NapcatWsServer(6001);
            var serverTask = server.StartAsync(cts.Token);

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
            cts.Cancel();
            await Task.WhenAll(clientTask, serverTask);
        }
    }
}
