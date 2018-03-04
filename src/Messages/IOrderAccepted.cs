using System.Collections;
using System.Collections.Generic;

namespace Messages
{
    public interface IOrderAccepted
    {
        IList<IProduct> Products { get; }
    }
}