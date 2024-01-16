using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using Telegram.Bot.Types;

namespace WBbot.Wildberries;

internal class WBAPIStat
{

    string Token;
    const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
    const string Urlsupplier = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
    Dictionary<string, string> keyValuePairsBarcode;
    Dictionary<string, string> keyValuePairsArticle;

    public WBAPIStat(string token)
    {
        Token = token;

        //this.botClient = botClient;
        using (StreamReader sr = new StreamReader("Wildberries\\Barcode.json"))
        {
            keyValuePairsBarcode = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
        }
        using (StreamReader sr = new StreamReader("Wildberries\\Article.json"))
        {
            keyValuePairsArticle = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
        }

    }


    public async Task<string?> SendStat(DateTime dateTime)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var query = new Dictionary<string, string>()
            {
                ["dateFrom"] = dateTime.ToString("O")
            };
            var Urlsupplier2 = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(Urlsupplier, query);
            var response = client.GetStringAsync(Urlsupplier2).Result;


            string message = "";
            int i = 1;
            var orders = JsonConvert.DeserializeObject<List<MainOrder>>(response);
            if (orders.Count == 0) { return message; }
            foreach (var item in orders)
            {
                message += $"{i++}) {item.date.AddHours(3).ToString("g")} {keyValuePairsBarcode[item.barcode]} price: {item.priceWithDisc}->{item.finishedPrice} \n";

            }
            return message;
            //await Console.Out.WriteLineAsync(orders[0].barcode);


        }
    }


    public async Task<string?> SendReport(DateTime dateTimeFrom, DateTime dateTimeTO)
    {
        string url = "https://statistics-api.wildberries.ru/api/v1/supplier/reportDetailByPeriod";
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var query = new Dictionary<string, string>()
            {
                ["dateFrom"] = dateTimeFrom.ToString("O"),
                ["dateTo"] = dateTimeTO.ToString("O")
            };
            url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);
            var response = client.GetStringAsync(url).Result;


            string message = "";
            int i = 1;
            var reports = JsonConvert.DeserializeObject<List<ReportStructMini>>(response);
            if (reports.Count == 0) { return message; }
            double all_Market = 0;
            double all_Sklad = 0;
            double? all_Logistic = 0;

            double ppvz_for_pay_Market = 0;
            double ppvz_for_pay_Sklad = 0;
            double rebill_logistic_cost = 0;


            foreach (var item in reports)
            {
                if (item.doc_type_name == "Продажа" & item.office_name == "Маркетплейс")
                {
                    all_Market += item.retail_amount;
                    ppvz_for_pay_Market += item.ppvz_for_pay;

                }
                else if (item.doc_type_name == "Продажа")
                {
                    all_Sklad += item.retail_amount;
                    ppvz_for_pay_Sklad += item.ppvz_for_pay;
                }
                else if (item.supplier_oper_name == "Логистика")
                {
                    all_Logistic += item.delivery_rub;
                    if (item.rebill_logistic_cost != null) { rebill_logistic_cost += item.rebill_logistic_cost; }
                }

            }
            message += $"маркет{all_Market} \t {ppvz_for_pay_Market}\n";

            message += $"склад {all_Sklad}\t {ppvz_for_pay_Sklad}\n";

            message += $"логистика{all_Logistic}\t{rebill_logistic_cost}\n";
            return message;



        }
    }


    // Получение информации об остатках на складе
    public async Task<string?> GetStocks()
    {

        var tokenMarket = "eyJhbGciOiJFUzI1NiIsImtpZCI6IjIwMjMxMDI1djEiLCJ0eXAiOiJKV1QifQ.eyJlbnQiOjEsImV4cCI6MTcxNzU2NDI5NCwiaWQiOiIyZTkyOTEwOC0xNzhkLTQ2NjEtOGE3My00YzE0YjY2ODQ5MzEiLCJpaWQiOjU3Njc4NTE5LCJvaWQiOjE0MjIzMzMsInMiOjEwNzM3NDE4NDAsInNpZCI6ImM0MjM1MmRjLTVkYjktNGVjMi1hZDViLWQ0ZTc4YTgzZjZiMiIsInVpZCI6NTc2Nzg1MTl9.ZZLdfKgj2NdSJyp4biirqYkVpTjsia9_fdQdWI4x74_BnTmGgt-0R8b0X05Cxvf76dBPwwoXQqHQgjm2URiSbw";

        // Устанавливаем URL для отправки HTTP-запроса
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/stocks";

        // Используем using для автоматического освобождения ресурсов после использования HttpClient
        using (var client = new HttpClient())
        {
            // Добавляем заголовок Authorization с токеном
            client.DefaultRequestHeaders.Add("Authorization", Token);

            // Создаем словарь с параметром "dateFrom", устанавливаем на текущую дату минус год
            var query = new Dictionary<string, string>()
            {
                ["dateFrom"] = DateTime.Now.AddYears(-1).ToString("O")
            };

            // Обновляем URL, добавляя параметры из словаря
            url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);

            try
            {
                // Отправляем GET-запрос к серверу и получаем ответ в виде строки
                var response = await client.GetStringAsync(url);

                // Инициализируем пустую строку для сообщения и переменную для подсчета элементов
                string message = "";
                int count = 1;

                // Десериализуем полученные данные в список объектов класса Stock
                var stocks = JsonConvert.DeserializeObject<List<Stock>>(response);

                // Проверяем, если список пуст, возвращаем "На складе пусто"
                if (stocks.Count == 0) { return "На складе пусто"; }

                // Добавляем в сообщение информацию об остатке на складе
                message += "Остаток на складе:\n";

                //Общий счетчик остатков на складе
                int all = 0;

                // Перебираем элементы списка stocks
                foreach (var stock in stocks)
                {
                    // Проверяем, если количество равно нулю, переходим к следующей итерации цикла
                    if (stock.quantity == 0) { continue; }

                    // Добавляем в сообщение информацию о каждом элементе в формате: Номер) Название -- Количество шт -- Название склада
                    message += $"{count++}) {keyValuePairsBarcode[stock.barcode]} -- {stock.quantity} шт -- ({stock.warehouseName})\n";
                    all += stock.quantity;
                }
                message += $"Ежедневный платеж за складское храниение {all*0.4} рублей";
                // Возвращаем сформированное сообщение
                return message;
            }
            catch (Exception ex)
            {
                // В случае возникновения ошибки, возвращаем null
                Console.WriteLine($"Ошибка при получении остатков на складе: {ex.Message}");
                string message = $"Ошибка при получении остатков на складе: {ex.Message}";
                return message;
            }
        }
    }


    public async Task<string?> GetIncomes()
    {
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/incomes";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var query = new Dictionary<string, string>()
            {
                ["dateFrom"] = DateTime.Now.AddDays(-7).ToString("O")
            };

            url = QueryHelpers.AddQueryString(url, query);

            try
            {
                var response = await client.GetStringAsync(url);

                var message = BuildIncomeMessage(response);
                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // В этом месте можно обработать исключение или вернуть сообщение об ошибке
                return null;
            }
        }
    }

    private string BuildIncomeMessage(string response)
    {
        string message = "";
        int i = 1;
        var Incomes = JsonConvert.DeserializeObject<List<Income>>(response);

        if (Incomes.Count == 0)
        {
            return "пусто";
        }

        message += $"Отчетное время: {DateTime.Now.AddHours(-3).ToString("g")}\n";

        foreach (var item in Incomes)
        {
            if (keyValuePairsBarcode.TryGetValue(item.barcode, out var itemName))
            {
                message += $"{i++}) {itemName} в количестве {item.quantity} Склад: {item.warehouseName}) \n";
            }
            else
            {
                // Обработка, если ключ не найден в словаре
                message += $"{i++}) Unknown в количестве {item.quantity} Склад: {item.warehouseName}) \n";
            }
        }

        return message;
    }

    public async Task<string?> GetAllOrders(DateTime dateTime)
    {
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
        try
        {
            // Create HttpClient with using statement to ensure proper disposal
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Token);

                // Append query string parameter for dateFrom
                var query = new Dictionary<string, string>()
                {
                    ["dateFrom"] = dateTime.ToString("O")
                };
                url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);

                // Use await instead of Result to correctly handle asynchronous operations
                var response = await client.GetStringAsync(url);

                string message = "";
                int i = 1;
                var orders = JsonConvert.DeserializeObject<List<StatOrder>>(response);

                if (orders.Count == 0) { return message = "пусто"; }

                message += $"Отчетное время: {dateTime.ToString("g")}\n";

                foreach (var item in orders)
                {
                    if (item.isCancel == true) { continue; }
                    else
                    {
                        message += $"{i++}) {item.date} -{keyValuePairsBarcode[item.barcode]} -- {item.priceWithDisc} р -- {item.regionName}\n";
                    }
                }

                return message;
            }
        }
        catch (Exception exception)
        {           
            Console.WriteLine(exception.Message);
            return exception.Message;
        }
    }
    public async Task<string?> GetAllOrdersCancel(DateTime dateTime)
    {
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
        try {
            // Create HttpClient with using statement to ensure proper disposal
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Token);

                // Append query string parameter for dateFrom
                var query = new Dictionary<string, string>()
                {
                    ["dateFrom"] = dateTime.ToString("O")
                };
                url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);

                // Use await instead of Result to correctly handle asynchronous operations
                var response = await client.GetStringAsync(url);

                string message = "";
                int i = 1;
                var orders = JsonConvert.DeserializeObject<List<StatOrder>>(response);

                if (orders.Count == 0) { return message = "пусто"; }

                message += $"Отчетное время: {dateTime.ToString("g")}\n";

                foreach (var item in orders)
                {
                    if (item.isCancel == true) { message += $"{i++}) {item.date} -{keyValuePairsBarcode[item.barcode]} -- {item.priceWithDisc} р {item.regionName}\n"; }
                    else
                    {
                        continue;
                    }
                }

                return message;
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            return exception.Message;
        }
    }

    public async Task<string?> GetAllSales(DateTime dateTime)
    {
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/sales";

        // Create HttpClient with using statement to ensure proper disposal
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", Token);

            // Append query string parameter for dateFrom
            var query = new Dictionary<string, string>()
            {
                ["dateFrom"] = dateTime.ToString("O")
            };
            url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);

            // Use await instead of Result to correctly handle asynchronous operations
            var response = await client.GetStringAsync(url);

            string message = "";
            int i = 1;
            var sales = JsonConvert.DeserializeObject<List<Sale>>(response);

            if (sales.Count == 0) { return message = "пусто"; }

            message += $"Отчетное время: {dateTime.ToString("g")}\n";

            foreach (var item in sales)
            {
                message += $"{i++}) {item.date} - {keyValuePairsBarcode[item.barcode]} -- {item.forPay} р -- {item.regionName}\n";
            }

            return message;
        }
    }

    public async Task<string?> AnalyticReport()
    {
        using (var client = new HttpClient())
        {
            string token = "eyJhbGciOiJFUzI1NiIsImtpZCI6IjIwMjMxMjI1djEiLCJ0eXAiOiJKV1QifQ.eyJlbnQiOjEsImV4cCI6MTcxOTQ3MTY3OCwiaWQiOiI0ZjhkMmVkYi00NGNiLTRhYzAtODIwMi1iMzliMzI0MzBmNjEiLCJpaWQiOjU3Njc4NTE5LCJvaWQiOjE0MjIzMzMsInMiOjQsInNpZCI6ImM0MjM1MmRjLTVkYjktNGVjMi1hZDViLWQ0ZTc4YTgzZjZiMiIsInQiOmZhbHNlLCJ1aWQiOjU3Njc4NTE5fQ.03u83eEG9QpGgVuwYxR5dWOuezkqcMsU7P7xqwUXeMSomUbt7V8T0b95QcLLU0AKzhyJ1kkkvxBI41ndQMhiyw";
            // HttpContent content = new StringContent("{\r\n  \"period\": {\r\n    \"begin\": \"2023-12-01 20:05:32\",\r\n    \"end\": \"2023-12-26 20:05:32\"\r\n  },\r\n  \"page\": 1\r\n}");
            // устанавливаем заголовок 
            string desiredTimeBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day-3).ToString("yyyy-MM-dd HH:mm:ss");
            string  desiredTimeEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1, 23, 59, 59).ToString("yyyy-MM-dd HH:mm:ss");
            
            Period period = new Period() { begin = desiredTimeBegin, end = desiredTimeEnd };
            Request request = new Request() { page = 1 , period = period };

            JsonContent content = JsonContent.Create(request);
            client.DefaultRequestHeaders.Add("Authorization", token);

            using var response = await client.PostAsync("https://suppliers-api.wildberries.ru/content/v1/analytics/nm-report/detail", content);
            string responseText = await response.Content.ReadAsStringAsync(); 
            var anal = JsonConvert.DeserializeObject<AnalyticSturust>(responseText);
            string message = $"С {desiredTimeBegin} по {desiredTimeEnd}\n";
            int i = 1;




            foreach (var item in anal.data.cards)
            {
                if (keyValuePairsArticle.ContainsKey(item.nmID.ToString()))
                {
                    message += $"{i++}) {keyValuePairsArticle[item.nmID.ToString()]}  {item.statistics.selectedPeriod.addToCartCount} -- card  -- {item.statistics.selectedPeriod.openCardCount} look\n";
                }
            }

            return message;


        }

        
    }




}

