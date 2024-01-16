using Newtonsoft.Json;
using Telegram.Bot;

namespace WBbot.Wildberries
{
    internal class APIWild
    {
        string Token;
        string TokenStat;
        const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
        const string Urlsupplier = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
        Dictionary<string, string> keyValuePairs;
        TelegramBotClient botClient;
        public APIWild(string token, string tokenStat)//, TelegramBotClient botClient)
        {
            Token = token;
            TokenStat = tokenStat;
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
            //await SendStat(DateTime.Now.AddDays(-4));
            int coolDown = 1000 * 60 * 30;
            long count = 1;
            while (true)
            {
                try
                {
                    var resp = GetRequest();

                    var orders = JsonConvert.DeserializeObject<Orders>(resp).orders;

                    if (orders != null & orders.Count == 0)
                    { await Console.Out.WriteLineAsync($"Проверка наличия новых заказов № {count++} ({DateTime.Now.ToString("G")})"); }
                    else
                    {
                        string message = "Было заказано:\n";
                        foreach (var order in orders)
                        {

                            message += keyValuePairs[order.skus[0]] + "\tСтоимость: " + order.price / 100 + " рублей\n Дата покупки: " + order.createdAt.AddHours(3) + "\n";
                        }
                        await botClient.SendTextMessageAsync("370802502", message);
                        await botClient.SendTextMessageAsync("388867563", message);

                        Thread.Sleep(coolDown);
                    }


                    Thread.Sleep(60000);

                }
                catch (Exception ex)
                {
                    // В случае возникновения ошибки, возвращаем null
                    Console.WriteLine($"Ошибка {ex.Message}");


                }





            }
        }


        public class Order
        {
            public object address { get; set; }
            public string deliveryType { get; set; }
            public object user { get; set; }
            public string orderUid { get; set; }
            public string article { get; set; }
            public string rid { get; set; }
            public DateTime createdAt { get; set; }
            public List<string> offices { get; set; }
            public List<string> skus { get; set; }
            public int id { get; set; }
            public int warehouseId { get; set; }
            public int nmId { get; set; }
            public int chrtId { get; set; }
            public int price { get; set; }
            public int convertedPrice { get; set; }
            public int currencyCode { get; set; }
            public int convertedCurrencyCode { get; set; }
            public int cargoType { get; set; }
        }

        public class Orders
        {
            public List<Order> orders { get; set; }
        }


    }
}
