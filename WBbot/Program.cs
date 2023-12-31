﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using WBbot;
using WBbot.Configuration;
using WBbot.Wildberries;

class Program
{
    static async Task Main()
    {
        // Создаем объект для работы с конфигурацией
        Configuration configuration = new Configuration();

        // Инициализируем экземпляры классов для работы с Wildberries API и Stat
        APIWild wild = new APIWild(configuration.WildberriesToken, configuration.WildberriesTokenStat);
        WBAPIStat stat = new WBAPIStat(configuration.WildberriesTokenStat);

        // Инициализируем строку для подключения к базе данных
        string connectionString = "Data Source =" + configuration.ConnectionStrings;

        // Инициализируем CancellationTokenSource для отмены задачи
        using CancellationTokenSource cts = new();

        // Инициализируем объект BotWorker для работы с Telegram Bot API
        BotWorker botWorker = new BotWorker(configuration.TelegramToken, cts, connectionString);

        // Передаем экземпляры классов APIWild и WBAPIStat внутрь объекта botWorker
        botWorker.APIWild = wild;
        botWorker.APIStat = stat;

        // Получаем информацию о боте с помощью Telegram Bot API
        var me = await botWorker.botClient.GetMeAsync();
        Console.WriteLine($"Start listening for @{me.Username}");

        // Отправляем сообщение через Wildberries API
        //await wild.SendMessage();

        // Отправляем статистику через Wildberries API
        // await wild.SendStat();

        // Ждем ввода с клавиатуры для завершения программы
        Console.ReadLine();

        // Отменяем задачу с помощью CancellationTokenSource
        cts.Cancel();
    }
}