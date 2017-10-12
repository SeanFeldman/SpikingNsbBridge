using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace MsmqEndpoint
{
    public class MyCommandHandler : IHandleMessages<MyCommand>
    {
        public Task Handle(MyCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine("Got a command");
            return Task.CompletedTask;
        }
    }
}