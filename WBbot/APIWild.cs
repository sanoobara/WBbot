using Newtonsoft.Json;
namespace WBbot
{
    internal class APIWild
    {
        string Token;
        const string Url = "https://suppliers-api.wildberries.ru/api/v3/orders/new";



        public APIWild(string token)
        {
            this.Token = token;


        }

        public string GetRequest()
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", this.Token);
                var response = client.GetStringAsync(Url).Result;
                Orders orders = JsonConvert.DeserializeObject<Orders>(response);


                return response;


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

