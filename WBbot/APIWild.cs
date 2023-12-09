using Newtonsoft.Json;
using System.Collections.Generic;
using Telegram.Bot;
namespace WBbot
{
    internal class APIWild
    {
        string Token;
        const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
        Dictionary<string, string> keyValuePairs;
        TelegramBotClient botClient;
        public APIWild(string token, TelegramBotClient botClient)
        {
            this.Token = token;
            this.botClient = botClient;
            using (StreamReader sr = new StreamReader("Barcode.json")) {
                this.keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
            }
            

        }
       
        private string GetRequest()
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", this.Token);
                var response = client.GetStringAsync(Url).Result;
               
                return response;

            }

        }

        public async Task SendMessage()
        {
            int coolDown = 1000 * 60 * 30;
            long count = 1;
            while (true)
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

                        message += this.keyValuePairs[order.skus[0]] + "\tСтоимость: " + order.price + "\n Дата покупки: " + order.createdAt + "\n";
                    }
                    await this.botClient.SendTextMessageAsync("370802502", message);
                    await this.botClient.SendTextMessageAsync("388867563", message);
                    Thread.Sleep(coolDown);
                }
                Thread.Sleep(3000);

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

