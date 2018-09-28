using System;
using System.Threading.Tasks;

namespace ServiceBusPOC.Sender.HTTP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
            Console.WriteLine("Sended...");
            Console.ReadKey();
        }
        
        private static async Task MainAsync()
        {
            var sas = new SharedAccessSignature("https://poc-sb-standard-tier.servicebus.windows.net/poc-standard-queue", "senders", "tQYnaPZ7E9uZwesQWBC89s1Bg/2zgTD33ojUEWxStYA=");

            var endpointUri = new Uri("https://poc-sb-standard-tier.servicebus.windows.net");

            var sender = new Sender(endpointUri, "poc-standard-queue", sas);

            await sender.SendMessageAsync("{\"name\":\"John Doe\",\"age\":32}");
        }
    }
}
