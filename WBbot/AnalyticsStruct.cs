using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBbot
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AdditionalError
    {
        public string field { get; set; }
        public string description { get; set; }
    }

    public class Card
    {
        public double nmID { get; set; }
        public string vendorCode { get; set; }
        public string brandName { get; set; }
        public List<Tag> tags { get; set; }
        public Object @object { get; set; }
        public Statistics statistics { get; set; }
        public Stocks stocks { get; set; }
    }

    public class Conversions
    {
        public double addToCartPercent { get; set; }
        public double cartToOrderPercent { get; set; }
        public double buyoutsPercent { get; set; }
    }

    public class Data
    {
        public double page { get; set; }
        public bool isNextPage { get; set; }
        public List<Card> cards { get; set; }
    }

    public class Object
    {
        public double id { get; set; }
        public string name { get; set; }
    }

    public class PeriodComparison
    {
        public double openCardDynamics { get; set; }
        public double addToCartDynamics { get; set; }
        public double ordersCountDynamics { get; set; }
        public double ordersSumRubDynamics { get; set; }
        public double buyoutsCountDynamics { get; set; }
        public double buyoutsSumRubDynamics { get; set; }
        public double cancelCountDynamics { get; set; }
        public double cancelSumRubDynamics { get; set; }
        public double avgOrdersCountPerDayDynamics { get; set; }
        public double avgPriceRubDynamics { get; set; }
        public Conversions conversions { get; set; }
    }

    public class PreviousPeriod
    {
        public string begin { get; set; }
        public string end { get; set; }
        public double openCardCount { get; set; }
        public double addToCartCount { get; set; }
        public double ordersCount { get; set; }
        public double ordersSumRub { get; set; }
        public double buyoutsCount { get; set; }
        public double buyoutsSumRub { get; set; }
        public double cancelCount { get; set; }
        public double cancelSumRub { get; set; }
        public double avgPriceRub { get; set; }
        public double avgOrdersCountPerDay { get; set; }
        public Conversions conversions { get; set; }
    }

    public class AnalyticSturust
    {
        public Data data { get; set; }
        public bool error { get; set; }
        public string errorText { get; set; }
        public List<AdditionalError>? additionalErrors { get; set; }
    }

    public class SelectedPeriod
    {
        public string begin { get; set; }
        public string end { get; set; }
        public double openCardCount { get; set; }
        public double addToCartCount { get; set; }
        public double ordersCount { get; set; }
        public double ordersSumRub { get; set; }
        public double buyoutsCount { get; set; }
        public double buyoutsSumRub { get; set; }
        public double cancelCount { get; set; }
        public double cancelSumRub { get; set; }
        public double avgPriceRub { get; set; }
        public double avgOrdersCountPerDay { get; set; }
        public Conversions conversions { get; set; }
    }

    public class Statistics
    {
        public SelectedPeriod selectedPeriod { get; set; }
        public PreviousPeriod previousPeriod { get; set; }
        public PeriodComparison periodComparison { get; set; }
    }

    public class Stocks
    {
        public double stocksMp { get; set; }
        public double stocksWb { get; set; }
    }

    public class Tag
    {
        public double id { get; set; }
        public string name { get; set; }
    }




    public class Period
    {
        public string begin { get; set; }
        public string end { get; set; }
    }

    public class Request
    {
        public Period period { get; set; }
        public double page { get; set; }
    }





}
