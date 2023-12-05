using Microsoft.Extensions.Configuration;
namespace WBbot
{
    internal class Configuration
    {

        public string TelegramToken { get; }
        public string WildberriesToken { get; }


        public static IConfiguration Iconfiguration { get; set; }
        public Configuration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
            Iconfiguration = builder.Build();
            this.TelegramToken = Iconfiguration["TelegramToken"];
            this.WildberriesToken = Iconfiguration["WildberriesToken"];
        }
    }
}
