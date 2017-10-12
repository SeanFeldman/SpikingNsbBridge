using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace AsbEndpoint
{
    public class MyEventHandler : IHandleMessages<MyEvent>
    {
        public Task Handle(MyEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine("Event received.");
            return Task.CompletedTask;
        }
    }
}