﻿using System;
using System.Collections.Generic;
using System.Linq;
using MassTransit;
using Messages;
using Shop;

IReadOnlyList<(string name, decimal price)> Products = new List<(string, decimal)>
{
    ("Bread", 1.20m),
    ("Milk", 0.50m),
    ("Rice", 1m),
    ("Pasta", 0.9m),
    ("Pasta", 0.9m),
    ("Cereals", 1.6m),
    ("Chocolate", 2m),
    ("Noodles", 1m),
    ("Pie", 1m),
    ("Sandwich", 1m),
};

var bus = Bus.Factory.CreateUsingAzureServiceBus(sbc =>
{
    sbc.Host("Endpoint=sb://tech-team-town-hall.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=48szyoAkRYCNwLxIzPbprxPr+2xoZn/OlZnsrU8TMVI=");

    sbc.ReceiveEndpoint("Shop", ep =>
    {
        ep.Consumer(() => new OrderRequestedFaultConsumer());
    });
});

bus.Start();

Console.WriteLine("Welcome to the Shop");
Console.WriteLine("Press Q key to exit");
Console.WriteLine("Press [0..9] key to order some products");
Console.WriteLine(string.Join(Environment.NewLine, Products.Select((x, i) => $"[{i}]: {x.name} @ {x.price:C}")));

var products = new List<(string name, decimal price)>();
for (;;)
{
    var consoleKeyInfo = Console.ReadKey(true);
    if (consoleKeyInfo.Key == ConsoleKey.Q)
    {
        break;
    }


    if (char.IsNumber(consoleKeyInfo.KeyChar))
    {
        // Hack: I don't care about ½ etc...
        var product = Products[(int)char.GetNumericValue(consoleKeyInfo.KeyChar)];
        products.Add(product);
        Console.WriteLine($"Added {product.name}");
    }

    if (consoleKeyInfo.Key == ConsoleKey.Enter)
    {
        bus.Publish<IOrderRequested>(new
        {
            Products = products.Select(x => new { Name = x.name, Price = x.price }).ToList()
        });

        Console.WriteLine("Submitted Order");

        products.Clear();
    }
}

bus.Stop();

