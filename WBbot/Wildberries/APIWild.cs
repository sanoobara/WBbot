using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public APIWild(string token, string tokenStat, TelegramBotClient botClient)
        {
            Token = token;
            TokenStat = tokenStat;
            this.botClient = botClient;
            using (StreamReader sr = new StreamReader("Barcode.json"))
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
            await SendStat();
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

                        message += keyValuePairs[order.skus[0]] + "\tСтоимость: " + order.price / 100 + " рублей\n Дата покупки: " + order.createdAt.AddHours(3) + "\n";
                    }
                    await botClient.SendTextMessageAsync("370802502", message);
                    await botClient.SendTextMessageAsync("388867563", message);

                    Thread.Sleep(coolDown);
                }

                Task.Delay(10000);
                Thread.Sleep(3000);

            }




        }


        public async Task SendStat()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", TokenStat);
                //client.DefaultRequestHeaders.Add("dateFrom", "2023-12-07T23:59:59");
                // AuthenticationHeaderValue authentication;

                //System.Net.Http.Headers.AuthenticationHeaderValue.TryParse(TokenStat, out authentication);
                //client.DefaultRequestHeaders.Authorization = authentication;
                //client.DefaultRequestHeaders.Add("Authorization", TokenStat);
                //client.DefaultRequestHeaders.Add("dateFrom", "2023-12-07T23:59:59");
                var query = new Dictionary<string, string>()
                {

                    ["dateFrom"] = "2023-12-07T23:59:59"
                };
                var Urlsupplier2 = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Urlsupplier, query);
                var response = client.GetStringAsync(Urlsupplier2).Result;

                //HttpClient httpClient = new HttpClient();
                //var serverAddress = Urlsupplier;
                //    using var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
                //    // устанавливаем оба заголовка
                //    request.Headers.Add("Authorization", TokenStat);
                //    request.Headers.Accept.Add (new MediaTypeWithQualityHeaderValue("application/json"));
                //    request.Headers.Add("dateFrom", "2023-12-07T23:59:59");



                //    using var response = await httpClient.SendAsync(request);
                //    var responseText = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response);
                var orders = JsonConvert.DeserializeObject<List<MainOrder>>(response);
                await Console.Out.WriteLineAsync(orders[0].barcode);

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
