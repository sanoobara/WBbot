namespace WBbot.Wildberries
{
    public class ReportStruct
    {
        public int realizationreport_id { get; set; }
        public DateTime date_from { get; set; }
        public DateTime date_to { get; set; }
        public DateTime create_dt { get; set; }
        public string currency_name { get; set; }
        public object suppliercontract_code { get; set; }
        public long rrd_id { get; set; }
        public int gi_id { get; set; }
        public string subject_name { get; set; }
        public int nm_id { get; set; }
        public string brand_name { get; set; }
        public string sa_name { get; set; }
        public string ts_name { get; set; }
        public string barcode { get; set; }
        public string doc_type_name { get; set; }
        public int quantity { get; set; }
        public int retail_price { get; set; }
        public int retail_amount { get; set; }
        public int sale_percent { get; set; }
        public double commission_percent { get; set; }
        public string office_name { get; set; }
        public string supplier_oper_name { get; set; }
        public DateTime order_dt { get; set; }
        public DateTime sale_dt { get; set; }
        public DateTime rr_dt { get; set; }
        public long shk_id { get; set; }
        public double retail_price_withdisc_rub { get; set; }
        public int delivery_amount { get; set; }
        public int return_amount { get; set; }
        public int delivery_rub { get; set; }
        public string gi_box_type_name { get; set; }
        public double product_discount_for_report { get; set; }
        public int supplier_promo { get; set; }
        public long rid { get; set; }
        public double ppvz_spp_prc { get; set; }
        public double ppvz_kvw_prc_base { get; set; }
        public double ppvz_kvw_prc { get; set; }
        public int sup_rating_prc_up { get; set; }
        public int is_kgvp_v2 { get; set; }
        public double ppvz_sales_commission { get; set; }
        public double ppvz_for_pay { get; set; }
        public double ppvz_reward { get; set; }
        public double acquiring_fee { get; set; }
        public string acquiring_bank { get; set; }
        public double ppvz_vw { get; set; }
        public double ppvz_vw_nds { get; set; }
        public int ppvz_office_id { get; set; }
        public string ppvz_office_name { get; set; }
        public int ppvz_supplier_id { get; set; }
        public string ppvz_supplier_name { get; set; }
        public string ppvz_inn { get; set; }
        public string declaration_number { get; set; }
        public string bonus_type_name { get; set; }
        public string sticker_id { get; set; }
        public string site_country { get; set; }
        public double penalty { get; set; }
        public int additional_payment { get; set; }
        public double rebill_logistic_cost { get; set; }
        public string rebill_logistic_org { get; set; }
        public string kiz { get; set; }
        public string srid { get; set; }
    }
    public class ReportStructMini
    {
        public string doc_type_name { get; set; }
        public double retail_amount { get; set; }
        public string office_name { get; set; }
        public double? delivery_rub { get; set; }
        public string supplier_oper_name { get; set; }
        public double ppvz_for_pay { get; set; }
        public double rebill_logistic_cost { get; set; }
    }

    public class Stock
    {
        public DateTime lastChangeDate { get; set; }
        public string warehouseName { get; set; }
        public string supplierArticle { get; set; }
        public int nmId { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public int inWayToClient { get; set; }
        public int inWayFromClient { get; set; }
        public int quantityFull { get; set; }
        public string category { get; set; }
        public string subject { get; set; }
        public string brand { get; set; }
        public string techSize { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public bool isSupply { get; set; }
        public bool isRealization { get; set; }
        public string SCCode { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Income
    {
        public int incomeId { get; set; }
        public string number { get; set; }
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string supplierArticle { get; set; }
        public string techSize { get; set; }
        public string barcode { get; set; }
        public int quantity { get; set; }
        public int totalPrice { get; set; }
        public DateTime dateClose { get; set; }
        public string warehouseName { get; set; }
        public int nmId { get; set; }
        public string status { get; set; }
    }



    public class Sale
    {
        public DateTime date { get; set; }
        public DateTime lastChangeDate { get; set; }
        public string warehouseName { get; set; }
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
        public int totalPrice { get; set; }
        public int discountPercent { get; set; }
        public int spp { get; set; }
        public double forPay { get; set; }
        public int finishedPrice { get; set; }
        public int priceWithDisc { get; set; }
        public string saleID { get; set; }
        public string orderType { get; set; }
        public string sticker { get; set; }
        public string gNumber { get; set; }
        public string srid { get; set; }
    }

}
