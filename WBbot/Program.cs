using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using WBbot;
using WBbot.Configuration;
using WBbot.Wildberries;
using WBbot.Wildberries.Marketplace;


class Program
{
    
    static async Task Main()
    {
        //var builder = WebApplication.CreateBuilder();
        //var app = builder.Build();
        //using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

        //ILogger logger = factory.CreateLogger("Program");
        //logger.LogInformation("Hello World! Logging is {Description}.", "fun");

        //var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();





        // Log.Logger = new LoggerConfiguration()
        //.WriteTo.Console()
        //.CreateBootstrapLogger();

        // Log.Information("Starting up!");


        //ILoggerFactory factory = new LoggerFactory();
        //ILogger<BotWorker> logger = factory.CreateLogger<BotWorker>();


        IServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ILogger<Program> logger = serviceProvider.GetService<ILogger<Program>>();

        //logger.LogInformation("Пример информационного сообщения");
        //logger.LogWarning("Пример предупреждения");
        //logger.LogError("Пример ошибки");
        //logger.LogCritical("Пример критической ошибки");

       



        // Создаем объект для работы с конфигурацией
        Configuration configuration = new Configuration();

        // Инициализируем экземпляры классов для работы с Wildberries API и Stat

        WBAPIStat stat = new WBAPIStat(configuration.WildberriesTokenStat);

        // Инициализируем строку для подключения к базе данных
        string connectionString = "Data Source =" + configuration.ConnectionStrings;

        // Инициализируем CancellationTokenSource для отмены задачи
        using CancellationTokenSource cts = new();

        // Инициализируем объект BotWorker для работы с Telegram Bot API
        BotWorker botWorker = new BotWorker(configuration.TelegramToken, cts, connectionString);
        MarketPlaceAPI wild = new MarketPlaceAPI(configuration.WildberriesToken, connectionString, botWorker.botClient);
        // Передаем экземпляры классов APIWild и WBAPIStat внутрь объекта botWorker
        botWorker.APIWild = wild;
        botWorker.APIStat = stat;

        // Получаем информацию о боте с помощью Telegram Bot API
        var me = await botWorker.botClient.GetMeAsync();
        Console.WriteLine($"Start listening for @{me.Username}");

        // Отправляем сообщение через Wildberries API
        await wild.SendMessage();

        // Отправляем статистику через Wildberries API
        // await wild.SendStat();

        // Ждем ввода с клавиатуры для завершения программы
        Console.ReadLine();

        // Отменяем задачу с помощью CancellationTokenSource
        cts.Cancel();
    }
}