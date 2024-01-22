using Newtonsoft.Json;
using Telegram.Bot;
using WBbot.DataBase;

namespace WBbot.Wildberries.Marketplace
{
    internal class MarketPlaceAPI
    {
        DBWorker dBWorker;

        string Token;
        string TokenStat;
        const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
        const string Urlsupplier = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
        Dictionary<string, string> keyValuePairs;
        TelegramBotClient botClient;
        public MarketPlaceAPI(string token, string connectionString, TelegramBotClient botClient)
        {
            dBWorker = new DBWorker(connectionString);

            this.botClient = botClient;
            Token = token;

            //this.botClient = botClient;
            using (StreamReader sr = new StreamReader("Wildberries\\Barcode.json"))
            {
                keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
            }
        }

        private string GetRequest()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Token);
                var response = client.GetStringAsync(Url).Result;
                return response;
            }

        }

        public async Task SendMessage()
        {

            List<string> list = new List<string>();
            //await SendStat(DateTime.Now.AddDays(-4));
            int coolDown = 1000 * 60 * 15;
            long count = 1;
            while (true)
            {
                int res=0;
                try
                {
                    var resp = GetRequest();

                    var orders = JsonConvert.DeserializeObject<Orders>(resp).orders;
                    await Console.Out.WriteLineAsync($"Проверка наличия новых заказов № {count++} ({DateTime.Now.ToString("G")})" );

                    if (orders != null & orders.Count == 0)
                    {  }
                    else
                    {
                        await Console.Out.WriteLineAsync($"Заказов {orders.Count} штук от {orders[0].createdAt.AddHours(3)}");
                        foreach (var order in orders)
                        {
                            res = dBWorker.AddMarketOrder(order.id, order.createdAt.AddHours(3), keyValuePairs[order.skus[0]], order.price / 100, order.convertedPrice, order.nmId).Result;
                            if (res > 0)
                            {
                                string message = "Было заказано:\n" + keyValuePairs[order.skus[0]] + "\tСтоимость: " + order.price / 100 + " рублей\n Дата покупки: " + order.createdAt.AddHours(3) + "\n";
                                await botClient.SendTextMessageAsync("370802502", message);
                                await botClient.SendTextMessageAsync("388867563", message);
                            }

                        }
                        
                       


                           /* string message = "Было заказано:\n";
                        foreach (var order in orders)
                        {

                            message += keyValuePairs[order.skus[0]] + "\tСтоимость: " + order.price / 100 + " рублей\n Дата покупки: " + order.createdAt.AddHours(3) + "\n";
                        }
                        await botClient.SendTextMessageAsync("370802502", message);
                        //await botClient.SendTextMessageAsync("388867563", message);*/

                        Thread.Sleep(coolDown);
                    }


                    Thread.Sleep(60000);

                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                }




            }

        }


    }


}

