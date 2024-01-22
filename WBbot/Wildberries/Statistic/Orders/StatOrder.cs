namespace WBbot.Wildberries.Statistic.Orders
{
    public class StatOrder
    {
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string? warehouseName { get; set; }
        public string countryName { get; set; }
        public string oblastOkrugName { get; set; }
        public string regionName { get; set; }
        public string supplierArticle { get; set; }
        public int nmId { get; set; }
        public string barcode { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public string brand { get; set; }
        public string techSize { get; set; }
        public int incomeID { get; set; }
        public bool isSupply { get; set; }
        public bool isRealization { get; set; }
        public double totalPrice { get; set; }
        public int discountPercent { get; set; }
        public double spp { get; set; }
        public double finishedPrice { get; set; }
        public double priceWithDisc { get; set; }
        public bool isCancel { get; set; }
        public DateTime cancelDate { get; set; }
        public string orderType { get; set; }
        public string sticker { get; set; }
        public string gNumber { get; set; }
        public string srid { get; set; }
    }
}
