using System.Collections.Generic;

namespace Messages
{
    public interface IOrderRequested
    {
        IList<IProduct> Products { get; }
    }
}
