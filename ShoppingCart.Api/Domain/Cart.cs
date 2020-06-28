using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;
using ShoppingCart.Api.Domain.Rules;
using System;

namespace ShoppingCart.Api.Domain
{
    public class Cart : BaseEntity, ICart
    {
        internal Dictionary<Product, double> ProductsWithQuantity { get; private set; }
        internal List<Coupon> Coupons { get; private set; }
        internal List<Campaign> Campaigns { get; private set; }

        public Cart()
        {
            ProductsWithQuantity = new Dictionary<Product, double>();
            Coupons = new List<Coupon>();
            Campaigns = new List<Campaign>();
        }

        public int GetDeliveryCount()
        {
            return ProductsWithQuantity.GroupBy(e => e.Key.Category.Title).Count();
        }

        public int GetProductCount()
        {
            return ProductsWithQuantity.Count;
        }

        public double GetTotalQuantity()
        {
            return ProductsWithQuantity.Sum(x => x.Value);
        }

        public double GetTotalAmount()
        {
            return ProductsWithQuantity.Sum(e => e.Key.Price * e.Value);
        }

        public double GetTotalAmountAfterDiscounts()
        {
            var totalAmount = GetTotalAmount();
            totalAmount -= CalculateCampaignDiscount(totalAmount);
            totalAmount -= CalculateCouponDiscount(totalAmount);
            return totalAmount;
        }

        public double GetCampaignDiscount()
        {
            return CalculateCampaignDiscount(GetTotalAmount());
        }

        public double GetCouponDiscount()
        {
            double totalAmount = GetTotalAmount();
            totalAmount -= CalculateCampaignDiscount(totalAmount);
            return CalculateCouponDiscount(totalAmount);
        }

        public void AddItem(Product product, double amount)
        {
            this.CheckRule(new ValueMustBeGreaterThanZeroRule(amount, "Added Product Amount"));
            this.CheckRule(new ObjectNotNullRule(product, "Added Product"));
            if (ProductsWithQuantity.TryGetValue(product, out double productAmount))
                ProductsWithQuantity[product] = productAmount + amount;
            else
                ProductsWithQuantity.Add(product, amount);
        }
        public void AddCampaings(params Campaign[] campaignArray)
        {
            this.CheckRule(new ObjectNotNullRule(campaignArray, "Added Campaings"));
            Campaigns.AddRange(campaignArray);
        }

        public void AddCoupons(params Coupon[] couponArray)
        {
            this.CheckRule(new ObjectNotNullRule(couponArray, "Added Coupons"));
            Coupons.AddRange(couponArray);
        }
        public string Print()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{"Category Name",15} {"Product Name",15} {"Quantity",10} {"Unit Price",12} {"Total Price",12}");

            var categoryWithProducts = ProductsWithQuantity.GroupBy(p => p.Key.Category.Title).ToDictionary(e => e.Key, e => e.ToList());
            foreach (var category in categoryWithProducts)
            {
                foreach (var product in category.Value)
                {
                    builder.AppendLine($"{category.Key,15} {product.Key.Title,15} {product.Value,10} {product.Key.Price,12} {(product.Key.Price * product.Value),12}\t");
                }
            }
            var totalAmount = GetTotalAmount();
            var totalAmountAfterDiscount = GetTotalAmountAfterDiscounts();
            var totalDeliveryCost = GetDeliveryCost();

            builder.AppendLine($"\nTotal Amount: {Math.Round(totalAmount, 2)}");
            builder.AppendLine($"Total Discount: {Math.Round(totalAmount - totalAmountAfterDiscount, 2)}");
            builder.AppendLine($"Total Amount After Discounts: {Math.Round(totalAmountAfterDiscount, 2)}");
            builder.AppendLine($"Delivery Cost: {Math.Round(totalDeliveryCost, 2)}");
            builder.AppendLine($"Payable Amount: {Math.Round((double)totalAmountAfterDiscount + (double)totalDeliveryCost, 2)}");

            return builder.ToString();
        }

        private double GetDeliveryCost()
        {
            var deliveryCostCalculator = new DeliveryCostCalculator(3.5, 2.5);
            return deliveryCostCalculator.Calculate(this);
        }

        private Dictionary<Product, double> GetProductsByCategory(Category category)
        {
            return ProductsWithQuantity.Where(p => p.Key.Category == category).ToDictionary(p => p.Key, p => p.Value);
        }

        private double CalculateCampaignDiscount(double totalAmount)
        {
            double discountAmount = 0;

            foreach (var campaign in Campaigns)
            {
                Dictionary<Product, double> products = GetProductsByCategory(campaign.Category);
                if (products.Values.Sum() >= campaign.MinimumQuantity)
                {
                    switch (campaign.DiscountType)
                    {
                        case DiscountType.Amount:
                            discountAmount = campaign.DiscountAmount > discountAmount ? campaign.DiscountAmount : discountAmount;
                            break;
                        case DiscountType.Rate:
                            double tempDiscountAmount = totalAmount * (campaign.DiscountAmount / 100);
                            discountAmount = tempDiscountAmount > discountAmount ? tempDiscountAmount : discountAmount;
                            break;
                        default:
                            break;
                    }
                }
            }

            return discountAmount;
        }

        private double CalculateCouponDiscount(double totalAmount)
        {
            double discountAmount = 0;

            foreach (var coupon in Coupons)
            {
                if (totalAmount >= coupon.MinimumAmount)
                {
                    switch (coupon.DiscountType)
                    {
                        case DiscountType.Rate:
                            discountAmount += totalAmount * (coupon.DiscountAmount / 100);
                            break;
                        case DiscountType.Amount:
                            discountAmount += coupon.DiscountAmount;
                            break;
                        default:
                            break;
                    }
                }
            }

            return discountAmount;
        }
    }
}