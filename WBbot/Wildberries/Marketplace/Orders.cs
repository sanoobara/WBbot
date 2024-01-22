
namespace WBbot.Wildberries.Marketplace
{

    public class Order
    {
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public List<string> skus { get; set; }
        public int price { get; set; }
        public int convertedPrice { get; set; }
        public int nmId { get; set; }

    }


    public class Orders
    {
        public List<Order> orders { get; set; }
    }
}
