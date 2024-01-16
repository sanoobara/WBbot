using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBbot.Wildberries.Marketplace
{
    public class Address
    {
        public string fullAddress { get; set; }
        public string province { get; set; }
        public string area { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string home { get; set; }
        public string flat { get; set; }
        public string entrance { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }

    public class Order
    {
        public int id { get; set; }
        public string rid { get; set; }
        public DateTime createdAt { get; set; }
        public int warehouseId { get; set; }
        public List<string> offices { get; set; }
        public Address address { get; set; }
        public List<string> requiredMeta { get; set; }
        public User user { get; set; }
        public List<string> skus { get; set; }
        public int price { get; set; }
        public int convertedPrice { get; set; }
        public int currencyCode { get; set; }
        public int convertedCurrencyCode { get; set; }
        public string orderUid { get; set; }
        public string deliveryType { get; set; }
        public int nmId { get; set; }
        public int chrtId { get; set; }
        public string article { get; set; }
        public int cargoType { get; set; }
    }

    public class User
    {
        public string fio { get; set; }
        public string phone { get; set; }
    }

    public class Orders
    {
        public List<Order> orders { get; set; }
    }
}
