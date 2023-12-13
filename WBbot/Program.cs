
using Telegram.Bot;
using WBbot;
using WBbot.Wildberries;

Configuration configuration = new Configuration();
APIWild wild = new APIWild(configuration.WildberriesToken, configuration.WildberriesTokenStat);//, botWorker.botClient);
WBAPIStat stat = new WBAPIStat(configuration.WildberriesTokenStat);
string connectionString = "Data Source =" + configuration.ConnectionStrings;
using CancellationTokenSource cts = new();
BotWorker botWorker = new BotWorker(configuration.TelegramToken, cts, connectionString);
botWorker.APIWild = wild;
botWorker.APIStat = stat;
var me = await botWorker.botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");



await wild.SendMessage();
//await wild.SendStat();

Console.ReadLine();
cts.Cancel();