using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Bridge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus.ConnectionString");
            var bridgeConfiguration = NServiceBus.Bridge.Bridge
                .Between<MsmqTransport>("Bridge-Publisher")
                .And<AzureServiceBusTransport>("Bridge-Subscriber", transport =>
                {
                    transport.ConnectionString(connectionString);
                    transport.UseForwardingTopology();
                });

            bridgeConfiguration.AutoCreateQueues();
            bridgeConfiguration.UseSubscriptionPersistece<InMemoryPersistence>((configuration, persistence) => { });

            var bridge = bridgeConfiguration.Create();

            await bridge.Start().ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            await bridge.Stop().ConfigureAwait(false);
        }
    }
}
