using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace AsbEndpoint
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new EndpointConfiguration("Subscriber");
            configuration.SendFailedMessagesTo("error");
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseSerialization<XmlSerializer>();
            var transport = configuration.UseTransport<AzureServiceBusTransport>();
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            transport.ConnectionString(connectionString);
            transport.UseForwardingTopology();

            var routing = transport.Routing();
            // todo: document that requires NServiceBus.Bridge.Connector
            var bridge = routing.ConnectToBridge("Bridge-Subscriber");
            bridge.RegisterPublisher(typeof(MyEvent), "Publisher");

            var endpoint = await Endpoint.Start(configuration).ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await endpoint.Stop().ConfigureAwait(false);
        }

    }
}
