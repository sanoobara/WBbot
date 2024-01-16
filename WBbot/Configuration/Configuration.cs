using Microsoft.Extensions.Configuration;
namespace WBbot.Configuration
{
    internal class Configuration
    {

        public string TelegramToken { get; }
        public string WildberriesToken { get; }
        public string WildberriesTokenStat { get; }
        public string ConnectionStrings { get; }


        public static IConfiguration Iconfiguration { get; set; }
        public Configuration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            Iconfiguration = builder.Build();
            TelegramToken = Iconfiguration["TelegramToken"];
            WildberriesToken = Iconfiguration["WildberriesToken"];
            WildberriesTokenStat = Iconfiguration["WildberriesTokenStat"];
            ConnectionStrings = Iconfiguration["ConnectionStrings"];

        }
    }
}
