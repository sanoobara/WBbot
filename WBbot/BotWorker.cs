using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WBbot.DataBase;
using WBbot.Wildberries;
using WBbot.Wildberries.Marketplace;

namespace WBbot
{
    internal class BotWorker
    {

        public TelegramBotClient botClient;
        DBWorker dBWorker;
        CancellationTokenSource cts;
        public MarketPlaceAPI APIWild { get; set; }
        public WBAPIStat APIStat { get; set; }

        public BotWorker(string Token, CancellationTokenSource cts, string connectionString)
        {

            botClient = new TelegramBotClient(Token);
            this.cts = cts;
            dBWorker = new DBWorker(connectionString);

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>(), // receive all update types except ChatMember related updates
                ThrowPendingUpdates = true // Херня которая говорит будет ли бот принимать сообщения пока он был офлайн
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        //async Task Starting(User user)
        //{
        //    if (user.LastName is null)
        //    {
        //        await dBWorker.AddUser(user.Username, user.Id, user.FirstName);
        //    }
        //    else
        //    {
        //        await dBWorker.AddUser(user.Username, user.Id, user.FirstName, user.LastName);
        //    }
        //}

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {           
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;
                        var user = message.From;

                        if (message.Text == "/start")
                        {
                            //await Starting(user);
                            await GetTestKeyboard(message);
                        }
                        break;
                    }
                case UpdateType.CallbackQuery:
                    {
                        var message = update.CallbackQuery;
                        var user = message.From;
                        if (message.Data == "Sales")
                        {
                            
                            var s = await APIStat.GetAllSales(DateTime.Now.AddDays(-7));
                            await botClient.SendTextMessageAsync(user.Id, s);
                            await dBWorker.AddUser(user.Username, user.Id, message.Data, s);
                            break;
                        }
                        if (message.Data == "Остатки")
                        {
                            
                            var s = await APIStat.GetStocks();
                            await botClient.SendTextMessageAsync(user.Id, s);
                            await dBWorker.AddUser(user.Username, user.Id, message.Data, s);
                            break;
                        }
                        if (message.Data == "incomes")
                        {
                            
                            var s = await APIStat.GetIncomes();
                            await botClient.SendTextMessageAsync(user.Id, s);
                            await dBWorker.AddUser(user.Username, user.Id, message.Data, s);
                            break;
                        }
                        if (message.Data == "Anal")
                        {
                            
                            var s = await APIStat.AnalyticReport();
                            await botClient.SendTextMessageAsync(user.Id, s);
                            await dBWorker.AddUser(user.Username, user.Id, message.Data, s);
                            break;
                        }
                        if (message.Data == "stat_order")
                        {
                            
                            var s = await APIStat.GetAllOrders(DateTime.Now.AddDays(-7));
                            await botClient.SendTextMessageAsync(user.Id, s);
                            await dBWorker.AddUser(user.Username, user.Id, message.Data, s);
                            break;
                        }
                        if (message.Data == "isCancel")
                        {
                            
                            var s = await APIStat.GetAllOrdersCancel(DateTime.Now.AddDays(-30));
                            await botClient.SendTextMessageAsync(user.Id, s);
                            await dBWorker.AddUser(user.Username, user.Id, message.Data, s);
                            break;
                        }

                        else
                        {
                            DateTime t;
                            DateTime.TryParse(message.Data, out t);
                            var s = await APIStat.SendStat(t);
                            await botClient.SendTextMessageAsync("370802502", s);
                            await botClient.SendTextMessageAsync("388867563", s);
                            break;
                        }
                    }

            }

            async Task GetTestKeyboard(Message message)
            {
                DateTime dateTime = DateTime.Now;

                InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
            // first row
            new []
            {
                
                InlineKeyboardButton.WithCallbackData(text: "Sales", callbackData: "Sales"),
                InlineKeyboardButton.WithCallbackData(text: "Остатки", callbackData: "Остатки"),

            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Стат по заказам", callbackData: "stat_order"),
                InlineKeyboardButton.WithCallbackData(text: "anal", callbackData: "Anal"),
                InlineKeyboardButton.WithCallbackData(text: "isCancel", callbackData: "isCancel"),
            },
            });

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Начало отсчета",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: this.cts.Token);
            }

        }


        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }

}
