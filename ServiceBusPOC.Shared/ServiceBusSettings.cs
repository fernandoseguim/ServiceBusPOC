using System.IO;
using Microsoft.Extensions.Configuration;

namespace ServiceBusPOC.Shared
{
    public class ServiceBusSettings
    {
        public static string ConnectionString => GetConnectionString();
        
        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            return configuration.GetConnectionString("ServiceBusConnectionString");
        }
    }
}
