using System;

namespace WebbutikM
{
    class EOutOfStock : Exception
    {
        public EOutOfStock(string message)
            : base(message)
        { }
    }
    class EItemNotFound : Exception
    {
        public EItemNotFound(string message)
            : base(message)
        { }
    }
}
