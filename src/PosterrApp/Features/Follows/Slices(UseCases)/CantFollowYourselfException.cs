using System;

namespace PosterrApp.Features.Follows
{
    public class CantFollowYourselfException : Exception
    {
        public CantFollowYourselfException(int quantityInStock, int amountToRemove)
            : base($"You cannot remove {amountToRemove} item(s) when there is only {quantityInStock} item(s) left.")
        {
            QuantityInStock = quantityInStock;
            AmountToRemove = amountToRemove;
        }

        public int QuantityInStock { get; }
        public int AmountToRemove { get; }
    }
}
