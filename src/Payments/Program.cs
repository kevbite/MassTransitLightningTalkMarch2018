using System;
using System.Linq;
using MassTransit;
using Payments;

var bus = Bus.Factory.CreateUsingAzureServiceBus(sbc =>
{
    sbc.Host("Endpoint=sb://tech-team-town-hall.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=48szyoAkRYCNwLxIzPbprxPr+2xoZn/OlZnsrU8TMVI=");

    sbc.ReceiveEndpoint("Payments", ep =>
    {
        ep.Consumer(() => new OrderRequestedConsumer());
    });
});

bus.Start();

Console.WriteLine("Welcome to Payments");
Console.WriteLine("Press Q key to exit");
while (Console.ReadKey(true).Key != ConsoleKey.Q) ;

bus.Stop();