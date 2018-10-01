using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;
using ServiceBusPOC.Shared;

namespace ServiceBusPOC.Queue.Sender
{
    internal class Program
    {
        private const string QUEUE_NAME = "poc-standard-queue";
        private static IQueueClient queueClient;

        private static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            const int NUMBER_OF_MESSAGES = 10;
            var tokenProvider = TokenProvider.CreateManagedServiceIdentityTokenProvider();
            
            
            //queueClient = new QueueClient(ServiceBusSettings.ConnectionString, QUEUE_NAME);

            AddHeader();

            await SendMessagesAsync(NUMBER_OF_MESSAGES);

            AddFooter();

            await queueClient.CloseAsync();
        }
        
        private static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    
                    var messageBody = $"Message { i + 1 }: Hello Azure Service Bus Messaging Queue";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    
                    Console.WriteLine($"Sending message: { messageBody }");

                    //Alternatively is possible schedule to send a message 
                    //await queueClient.ScheduleMessageAsync( message, new DateTimeOffset( DateTime.Now.AddSeconds( 10 )));

                    await queueClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{ DateTime.Now } :: Exception: { exception.Message }");
            }
        }

        private static void AddFooter()
        {
            Console.WriteLine();
            Console.WriteLine("Messages were sent with successful!");

            Console.ReadKey();
        }

        private static void AddHeader()
        {
            Console.WriteLine($"Connection String: { ServiceBusSettings.ConnectionString }");
            Console.WriteLine($"Queue: { QUEUE_NAME }");
            Console.WriteLine("=========================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("=========================================================");
        }
    }
}
