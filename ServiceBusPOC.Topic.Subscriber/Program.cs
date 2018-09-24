using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBusPOC.Shared;

namespace ServiceBusPOC.Topic.Subscriber
{
    internal class Program
    {
        private const string TOPIC_NAME = "service-bus-topic-poc";
        private const string SUBSCRIPTION_NAME = "poc-subscription";
        private static ISubscriptionClient subscriptionClient;

        private static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            subscriptionClient = new SubscriptionClient(ServiceBusSettings.ConnectionString, TOPIC_NAME, SUBSCRIPTION_NAME);

            AddHeader();
            
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await subscriptionClient.CloseAsync();
        }

        private static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 1, AutoComplete = false };
            
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{ message.SystemProperties.SequenceNumber }"
                            + $" Body:{ Encoding.UTF8.GetString(message.Body) }");

            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);    
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception { exceptionReceivedEventArgs.Exception }.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: { context.Endpoint }");
            Console.WriteLine($"- Entity Path: { context.EntityPath }");
            Console.WriteLine($"- Executing Action: { context.Action }");
            return Task.CompletedTask;
        }

        private static void AddHeader()
        {
            Console.WriteLine($"Connection String: { ServiceBusSettings.ConnectionString }");
            Console.WriteLine($"TOPIC: { TOPIC_NAME }");
            Console.WriteLine();
            Console.WriteLine("=========================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("=========================================================");
        }
    }
}
