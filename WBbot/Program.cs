
using Telegram.Bot;
using WBbot;

Configuration configuration = new Configuration();

APIWild wild = new APIWild(configuration.WildberriesToken);

string connectionString = "Data Source =" + configuration.ConnectionStrings;
using CancellationTokenSource cts = new();
BotWorker botWorker = new BotWorker(configuration.TelegramToken, cts, connectionString);
var me = await botWorker.botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();


//Task.Delay(1000).Wait();
Console.WriteLine(wild.GetRequest());