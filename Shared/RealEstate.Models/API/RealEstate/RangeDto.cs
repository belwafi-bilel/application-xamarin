using System;
namespace RealEstate.Models.API.RealEstate
{
    public class RangeDto
    {
        public double From { get; set; }
        public double To { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RangeDto item))
            {
                return false;
            }
            return this.From.Equals(item.From) && this.To .Equals(item.To);
        }
    }

    public enum TypeEnum
    {
        Sale,
        Rent,
        None
    }
}
