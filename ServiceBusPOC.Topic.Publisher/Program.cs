using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using ServiceBusPOC.Shared;

namespace ServiceBusPOC.Topic.Publisher
{
    internal class Program
    {
        private const string TOPIC_NAME = "service-bus-topic-poc";
        private static ITopicClient topicClient;

        private static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            const int NUMBER_OF_MESSAGES = 10;
            topicClient = new TopicClient(ServiceBusSettings.ConnectionString, TOPIC_NAME);

            AddHeader();
            
            await SendMessagesAsync(NUMBER_OF_MESSAGES);

            Console.ReadKey();

            await topicClient.CloseAsync();
        }

        private static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                var messages = new List<Message>();

                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    var messageBody = $"Message { i }: Hello Azure Service Bus Messaging Topic and Subscription";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    
                    messages.Add(message);
                }

                Console.WriteLine($"Batch messages was sent with successful!");
                await topicClient.SendAsync(messages);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{ DateTime.Now } :: Exception: { exception.Message }");
            }
        }

        private static void AddHeader()
        {
            Console.WriteLine($"Connection String: { ServiceBusSettings.ConnectionString }");
            Console.WriteLine($"TOPIC: { TOPIC_NAME }");
            Console.WriteLine();
            Console.WriteLine("=======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("=======================================================");
        }
    }
}
