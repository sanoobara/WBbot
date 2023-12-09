
using Telegram.Bot;
using WBbot;

Configuration configuration = new Configuration();


string connectionString = "Data Source =" + configuration.ConnectionStrings;
using CancellationTokenSource cts = new();
BotWorker botWorker = new BotWorker(configuration.TelegramToken, cts, connectionString);
var me = await botWorker.botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");


APIWild wild = new APIWild(configuration.WildberriesToken, botWorker.botClient);
wild.SendMessage();
Console.ReadLine();
cts.Cancel();