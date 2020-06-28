namespace ShoppingCart.Api.Domain.Interfaces
{
    public interface ICart
    {
        int GetDeliveryCount();
        int GetProductCount();
        double GetTotalQuantity();
        double GetTotalAmount();
        double GetTotalAmountAfterDiscounts();
        double GetCampaignDiscount();
        double GetCouponDiscount();
        void AddItem(Product product, double amount);
        void AddCampaings(params Campaign[] campaignArray);
        void AddCoupons(params Coupon[] couponArray);
        string Print();
    }
}