
using Telegram.Bot;
using WBbot;
using WBbot.Wildberries;

Configuration configuration = new Configuration();


string connectionString = "Data Source =" + configuration.ConnectionStrings;
using CancellationTokenSource cts = new();
BotWorker botWorker = new BotWorker(configuration.TelegramToken, cts, connectionString);
var me = await botWorker.botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");


APIWild wild = new APIWild(configuration.WildberriesToken, configuration.WildberriesTokenStat, botWorker.botClient);
await wild.SendMessage();
//await wild.SendStat();

Console.ReadLine();
cts.Cancel();