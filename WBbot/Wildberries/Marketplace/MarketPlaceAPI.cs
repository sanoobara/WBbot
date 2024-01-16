using Newtonsoft.Json;
using Telegram.Bot;

namespace WBbot.Wildberries.Marketplace
{
    internal class MarketPlaceAPI
    {
        string Token;
        string TokenStat;
        const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
        const string Urlsupplier = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
        Dictionary<string, string> keyValuePairs;
        TelegramBotClient botClient;
        public MarketPlaceAPI(string token, string tokenStat)//, TelegramBotClient botClient)
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

            List<string> list = new List<string>();
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
                        DirectoryInfo dI = new DirectoryInfo("MarketOrders");
                        if (!dI.Exists) { dI.Create(); }


                        if (list.Contains(orders[0].rid)) { continue; }
                        else
                        {
                            FileInfo fileInfo = new FileInfo(DateTime.Now.ToString("f"));
                            fileInfo.Create();
                            TextWriter textWriter = new StreamWriter(fileInfo.Name);
                            await textWriter.WriteAsync(resp);

                            list.Add(orders[0].rid);

                            string message = "Было заказано:\n";
                            foreach (var order in orders)
                            {

                                message += keyValuePairs[order.skus[0]] + "\tСтоимость: " + order.price / 100 + " рублей\n Дата покупки: " + order.createdAt.AddHours(3) + "\n";
                            }
                            await botClient.SendTextMessageAsync("370802502", message);
                            await botClient.SendTextMessageAsync("388867563", message);

                            Thread.Sleep(coolDown);
                        }
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

