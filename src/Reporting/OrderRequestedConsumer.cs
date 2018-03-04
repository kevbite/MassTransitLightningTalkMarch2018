using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace Reporting
{
    public class OrderRequestedConsumer : IConsumer<IOrderRequested>
    {
        private readonly ReportStore _store;

        public OrderRequestedConsumer(ReportStore store)
        {
            _store = store;
        }

        public Task Consume(ConsumeContext<IOrderRequested> context)
        {
            _store.IncrementTotalOrdersRequested(context.Message.Products.Sum(x => x.Price));

            return Task.CompletedTask;
        }
    }
}