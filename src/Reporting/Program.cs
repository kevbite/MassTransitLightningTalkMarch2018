using System;
using System.Linq;
using MassTransit;

namespace Reporting
{
    class Program
    {
        static void Main(string[] args)
        {
            var reportStore = new ReportStore();
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                sbc.ReceiveEndpoint(host, "Reporting", ep =>
                {
                    ep.Consumer(() => new OrderRequestedConsumer(reportStore));
                    ep.Consumer(() => new OrderAcceptedConsumer(reportStore));
                });
            });

            bus.Start();
            
            Console.WriteLine("Welcome to Reports");
            Console.WriteLine("Press Q key to exit");
            Console.WriteLine("Press R key to show report");

            for (;;)
            {
                var consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Q) break;
                if (consoleKeyInfo.Key == ConsoleKey.R)
                {
                    Console.WriteLine("-- Product Sales --");
                    Console.WriteLine(string.Join(Environment.NewLine, reportStore.ProductSales.Select(x => $"{x.Key}: {x.Value:C}")));
                    
                    Console.WriteLine("-- Totals --");
                    Console.WriteLine($"TotalOrdersRequested: {reportStore.TotalOrdersRequested:C}");
                    Console.WriteLine($"TotalOrdersAccepted: {reportStore.TotalOrdersAccepted:C}");
                }
            }

            bus.Stop();
        }
    }
}
