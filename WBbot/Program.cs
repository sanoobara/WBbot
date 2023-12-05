
using WBbot;

Configuration configuration = new Configuration();

APIWild wild = new APIWild(configuration.WildberriesToken);


Task.Delay(1000).Wait();
Console.WriteLine(wild.GetRequest());