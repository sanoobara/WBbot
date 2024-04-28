using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBbot
{
    internal class Logger
    {

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();

            });
        }


        //IServiceCollection services = new ServiceCollection();
        //Program.ConfigureServices(services);
        //    IServiceProvider serviceProvider = services.BuildServiceProvider();
        //ILogger<BotWorker> logger = serviceProvider.GetService<ILogger<BotWorker>>();



    }
}
