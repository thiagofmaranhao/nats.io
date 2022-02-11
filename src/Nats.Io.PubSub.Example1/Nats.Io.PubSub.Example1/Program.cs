using System;
using System.Text;
using System.Threading;
using NATS.Client;

namespace Nats.Io.PubSub.Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Conexão
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            
            // Subscription
            EventHandler<MsgHandlerEventArgs> messageHandler = (sender, args) =>
            {
                Console.WriteLine($"{DateTime.Now:F} - Received: {args.Message}");
            };

            var subscribeAsync = connection.SubscribeAsync("foo");
            subscribeAsync.MessageHandler += messageHandler;
            subscribeAsync.Start();
            
            // Publish
            var message = "Hello World!";
            Console.WriteLine($"{DateTime.Now:F} - Send: {message}");
            connection.Publish("foo", Encoding.UTF8.GetBytes(message));
            
            Thread.Sleep(1000);
            
            // Disconnect
            subscribeAsync.Unsubscribe();
            connection.Drain();
            connection.Close();

            Console.ReadLine();
        }
    }
}