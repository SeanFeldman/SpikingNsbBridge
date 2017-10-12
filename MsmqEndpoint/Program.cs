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

            Console.WriteLine("Press <enter> to send a message");
            Console.WriteLine("Press any other key to exit");
            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    break;
                }
                //                await endpoint.Send(new MyCommand()).ConfigureAwait(false);
                //                Console.WriteLine("Command sent");
                await endpoint.Publish<MyEvent>(@event => { }).ConfigureAwait(false);
                Console.WriteLine("Event sent");
            }

            await endpoint.Stop().ConfigureAwait(false);
        }

    }
}
