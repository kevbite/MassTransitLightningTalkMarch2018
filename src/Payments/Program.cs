using System;
using System.Linq;
using MassTransit;
using Payments;

var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
{
    sbc.Host(new Uri("rabbitmq://localhost"), h =>
    {
        h.Username("guest");
        h.Password("guest");
    });

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