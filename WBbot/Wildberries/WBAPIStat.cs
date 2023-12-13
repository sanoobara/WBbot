using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace WBbot.Wildberries;

internal class WBAPIStat
{

    string Token;
    const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";
    const string Urlsupplier = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";
    Dictionary<string, string> keyValuePairs;

    public WBAPIStat(string token)
    {
        Token = token;

        //this.botClient = botClient;
        using (StreamReader sr = new StreamReader("Barcode.json"))
        {
            keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
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
                message += $"{i++}) {item.date.AddHours(3).ToString("g")} {keyValuePairs[item.barcode]} price: {item.priceWithDisc}->{item.finishedPrice} \n";

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

        public async Task<string?> GetStoks()
        {
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/stocks";
            
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", Token);

                var query = new Dictionary<string, string>()
                {
                    ["dateFrom"] = DateTime.UtcNow.ToString("O")
                };
                url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);
                var response = client.GetStringAsync(url).Result;


                string message = "";
                int i = 1;
                var stocks = JsonConvert.DeserializeObject<List<Stock>>(response);
                if (stocks.Count == 0) { return message = "На складе пусто"; }
            message += $"Отчетное время: {DateTime.Now.ToString("g")}\n";
                foreach (var item in stocks)
                {
                    message += $"{i++}) {keyValuePairs[item.barcode]} в количестве {item.quantity} ({item.quantityFull}) по цене {item.Price}) \n";

                }
                return message;
                //await Console.Out.WriteLineAsync(orders[0].barcode);


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
            url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);
            var response = client.GetStringAsync(url).Result;


            string message = "";
            int i = 1;
            var Incomes = JsonConvert.DeserializeObject<List<Income>>(response);
            if (Incomes.Count == 0) { return message = "пусто"; }
            message += $"Отчетное время: {DateTime.Now.AddHours(-3).ToString("g")}\n";
            foreach (var item in Incomes)
            {
                message += $"{i++}) {keyValuePairs[item.barcode]} в количестве {item.quantity} Склад: {item.warehouseName}) \n";

            }
            return message;
            //await Console.Out.WriteLineAsync(orders[0].barcode);


        }
    }

    public async Task<string?> GetAllOrders(DateTime dateTime)
    {
        var url = "https://statistics-api.wildberries.ru/api/v1/supplier/orders";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var query = new Dictionary<string, string>()
            {
                ["dateFrom"] = dateTime.ToString("O")
            };
            url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(url, query);
            var response = client.GetStringAsync(url).Result;


            string message = "";
            int i = 1;
            var orders = JsonConvert.DeserializeObject<List<StatOrder>>(response);
            if (orders.Count == 0) { return message = "пусто"; }
            message += $"Отчетное время: {dateTime.ToString("g")}\n";
            foreach (var item in orders)
            {
                message += $"{i++}) {item.date} -{keyValuePairs[item.barcode]} -- {item.priceWithDisc} руб, откуда: {item.warehouseName} куда {item.regionName}  \n";

            }
            return message;
            //await Console.Out.WriteLineAsync(orders[0].barcode);


        }
    }


}

