using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WBbot
{
    internal class BotWorker
    {

        public TelegramBotClient botClient;
        DBWorker dBWorker;

        public BotWorker(string Token, CancellationTokenSource cts, string connectionString)
        {

            botClient = new TelegramBotClient(Token);

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


        async Task Starting(User user)
        {
            if (user.LastName is null)
            {
                await dBWorker.AddUser(user.Username, user.Id, user.FirstName);
            }
            else
            {
                await dBWorker.AddUser(user.Username, user.Id, user.FirstName, user.LastName);
            }
          
        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            // Only process Message updates: https://core.telegram.org/bots/api#message
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;
                        var user = message.From;

                        if (message.Text == "/start")
                        {
                           await Starting(user);
                            await Console.Out.WriteLineAsync("456");
                        }



                        break;
                    }


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
