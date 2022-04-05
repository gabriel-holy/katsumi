using System;
using System.Runtime.Serialization;

namespace PosterrApp.Features.Follows
{
    [Serializable]
    internal class NotEnoughStockException : Exception
    {
        private int quantityInStock;
        private int amount;

        public NotEnoughStockException()
        {
        }

        public NotEnoughStockException(string message) : base(message)
        {
        }

        public NotEnoughStockException(int quantityInStock, int amount)
        {
            this.quantityInStock = quantityInStock;
            this.amount = amount;
        }

        public NotEnoughStockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}