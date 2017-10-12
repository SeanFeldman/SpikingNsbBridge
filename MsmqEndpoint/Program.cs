using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace MsmqEndpoint
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new EndpointConfiguration("Publisher");
            configuration.SendFailedMessagesTo("error");
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            var transport = configuration.UseTransport<MsmqTransport>();

            var routing = transport.Routing();
            // todo: document that requires NServiceBus.Bridge.Connector
            var bridge = routing.ConnectToBridge("Bridge-Publisher");
            bridge.RouteToEndpoint(typeof(MyCommand), "Subscriber");
            
            var endpoint = await Endpoint.Start(configuration).ConfigureAwait(false);

            Console.WriteLine("Press 'c' to send a command and 'e' to publish an event.");
            Console.WriteLine("Press any other key to exit");
            var @continue = true;

            while (@continue)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.C:
                        await endpoint.Send(new MyCommand()).ConfigureAwait(false);
                        Console.WriteLine("\nCommand sent");
                        break;

                    case ConsoleKey.E:
                        await endpoint.Publish<MyEvent>(@event => { }).ConfigureAwait(false);
                        Console.WriteLine("\nEvent sent");
                        break;

                    default:
                        @continue = false;
                        break;
                }
            }

            await endpoint.Stop().ConfigureAwait(false);
        }

    }
}
