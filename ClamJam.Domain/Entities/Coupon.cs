using ClamJam.Domain.DomainModels;

namespace ClamJam.Domain.Entities
{
    public abstract class Coupon
    {
        public EnumCouponCode Code { get; }
        public Percentage DiscountPercetnage { get; }

        protected Coupon(EnumCouponCode code, Percentage discount)
        {
            Code = code;
            DiscountPercetnage = discount;
        }


        /// <summary>
        /// Discounted amount after applying coupon 
        /// </summary>
        /// <param name="subtotal">Initial sum on which the coupon is applied</param>
        /// <returns></returns>
        public abstract Money GetDiscountedAmount(Money subtotal);

        public override bool Equals(object? obj)
        {
            return obj is Coupon other && Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }

    public enum EnumCouponCode
    {
        Free,
        Percentage
    }

    public class FreeCoupon : Coupon
    {
        public FreeCoupon() : base(EnumCouponCode.Free, new Percentage(100)) { }


        public override Money GetDiscountedAmount(Money subtotal)
        {
            return subtotal.Multiply(DiscountPercetnage.Factor);
        }
    }

    public class PercentageCoupon : Coupon
    {


        public PercentageCoupon(Percentage discount) : base(EnumCouponCode.Percentage, discount) { }

        public override Money GetDiscountedAmount(Money subtotal)
        {
           return subtotal.Multiply(DiscountPercetnage.Factor);
        }
    }

    public static class CouponFactory
    {
        public static Coupon? Create(EnumCouponCode? couponCode, Percentage? percentage)
        {

            if (couponCode == EnumCouponCode.Free)
                return new FreeCoupon();

            if (couponCode == EnumCouponCode.Percentage)
            {
                if (!percentage.HasValue)
                {
                    throw new ArgumentException("Percentage cannot be null or empty");
                }
       
                if (percentage >= 0 && percentage <= 100)
                    return new PercentageCoupon(percentage.Value);
            }

            return null;
        }
    }
}
