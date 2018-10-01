using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusPOC.Shared;

namespace ServiceBusPOC.Queue.Receiver
{
    internal class Program
    {
        private const string QUEUE_NAME = "poc-basic-queue";
        private static IQueueClient queueClient;

        private static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            queueClient = new QueueClient(ServiceBusSettings.ConnectionString, QUEUE_NAME);

            AddHeader();

            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await queueClient.CloseAsync();
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 1, AutoComplete = false };
            
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var order = JsonConvert.DeserializeObject<Order>(Encoding.UTF8.GetString(message.Body));

            Console.WriteLine($"New order:{ order.OrderId }");
            
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine();
            Console.WriteLine($" Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
        
        private static void AddHeader()
        {
            Console.WriteLine($"Connection String: { ServiceBusSettings.ConnectionString }");
            Console.WriteLine($"Queue: { QUEUE_NAME }");
            Console.WriteLine();
            Console.WriteLine("=========================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("=========================================================");
        }
    }
}
