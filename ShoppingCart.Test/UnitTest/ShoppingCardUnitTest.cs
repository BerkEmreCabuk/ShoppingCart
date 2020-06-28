using System;
using ShoppingCart.Api.Domain;
using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;
using ShoppingCart.Api.Infrastructures.Exceptions;
using Xunit;

namespace ShoppingCart.Test.UnitTest
{
    public class ShoppingCartUnitTest
    {
        private ICart _cart;

        public ShoppingCartUnitTest(
        )
        {
            _cart = new Cart();
        }

        #region AddItem

        [Fact]
        public void AddItem_ProductWithQuantity_ShouldAdd()
        {
            var product = new Product("Apple", 2, new Category("Food"));

            _cart.AddItem(product, 40);

            Assert.Equal(1, _cart.GetProductCount());
            Assert.Equal(40, _cart.GetTotalQuantity());
        }

        [Fact]
        public void AddItem_SameProductDifferentQuantity_ShouldAdd()
        {
            var product = new Product("Apple", 2, new Category("Food"));

            _cart.AddItem(product, 20);
            _cart.AddItem(product, 40);

            Assert.Equal(1, _cart.GetProductCount());
            Assert.Equal(60, _cart.GetTotalQuantity());
        }

        [Fact]
        public void AddItem_DifferentProductDifferentQuantity_ShouldAdd()
        {
            var product1 = new Product("Apple", 2, new Category("Food"));
            var product2 = new Product("Banana", 2, new Category("Food"));

            _cart.AddItem(product1, 20);
            _cart.AddItem(product1, 40);
            _cart.AddItem(product2, 20);
            _cart.AddItem(product2, 40);

            Assert.Equal(2, _cart.GetProductCount());
            Assert.Equal(120, _cart.GetTotalQuantity());
        }

        [Fact]
        public void AddItem_NullProduct_ReturnException()
        {
            Product product = null;

            Assert.Throws<ArgumentNullException>(() => _cart.AddItem(product, 1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddItem_ProductWithZeroOrNegativeQuantity_ReturnException(int quantity)
        {
            var product = new Product("Apple", 2, new Category("Food"));

            Assert.Throws<NotificationException>(() => _cart.AddItem(product, quantity));
        }
        #endregion
        #region  AddCampaings
        [Theory]
        [InlineData(6, 5, 100, 100, DiscountType.Amount)]
        [InlineData(4, 5, 100, 0, DiscountType.Amount)]
        [InlineData(6, 5, 10, 60, DiscountType.Rate)]
        [InlineData(4, 5, 10, 0, DiscountType.Rate)]
        public void AddCampaings_ValidCampaingDiscountTypeAmount_ShouldApply(int productQuantity, int minimumQuantity, double discountAmount, double expectedDiscountAmount, DiscountType discountType)
        {
            Category category = new Category("Food");
            Product product = new Product("Apple", 100, category);
            _cart.AddItem(product, productQuantity);

            Campaign campaign = new Campaign(category, minimumQuantity, discountAmount, discountType);
            _cart.AddCampaings(campaign);

            Assert.Equal(expectedDiscountAmount, _cart.GetCampaignDiscount());
        }

        [Fact]
        public void AddCampaings_NullCampaings_ReturnException()
        {
            Assert.Throws<ArgumentNullException>(() => _cart.AddCampaings(null));
        }

        #endregion
        #region  AddCoupons
        [Theory]
        [InlineData(1, 2500, 100, 100, DiscountType.Amount)]
        [InlineData(1, 3500, 100, 0, DiscountType.Amount)]
        [InlineData(1, 2500, 10, 300, DiscountType.Rate)]
        [InlineData(1, 3500, 10, 0, DiscountType.Rate)]
        public void AddCoupons_ValidCouponDiscountTypeAmount_ShouldApply(int productQuantity, int minimumAmount, double discountAmount, double expectedDiscountAmount, DiscountType discountType)
        {
            Category category = new Category("SmartPhone");
            Product product = new Product("IPhoneX", 3000, category);
            _cart.AddItem(product, productQuantity);

            Coupon coupon = new Coupon(minimumAmount, discountAmount, discountType);
            _cart.AddCoupons(coupon);

            Assert.Equal(expectedDiscountAmount, _cart.GetCouponDiscount());
        }

        [Fact]
        public void AddCoupons_NullCoupon_ReturnException()
        {
            Assert.Throws<ArgumentNullException>(() => _cart.AddCoupons(null));
        }

        #endregion
        #region GetTotalAmount
        [Fact]
        public void GetTotalAmount_EmptyProduct_ReturnsZero()
        {
            Assert.Equal(0, _cart.GetTotalAmount());
        }

        [Fact]
        public void GetTotalAmount_SameProduct_ReturnTrueAmount()
        {
            Category category = new Category("Food");
            Product product = new Product("Apple", 2, category);

            _cart.AddItem(product, 10);
            _cart.AddItem(product, 5);

            Assert.Equal(30, _cart.GetTotalAmount());
        }

        [Fact]
        public void GetTotalAmount_DifferentProduct_ReturnTrueAmount()
        {
            Category category = new Category("Food");
            Product product1 = new Product("Apple", 2, category);
            Product product2 = new Product("Banana", 4, category);

            _cart.AddItem(product1, 10);
            _cart.AddItem(product1, 5);
            _cart.AddItem(product2, 10);
            _cart.AddItem(product2, 5);

            Assert.Equal(90, _cart.GetTotalAmount());
        }
        #endregion
        #region GetTotalAmountAfterDiscounts
        [Fact]
        public void GetTotalAmountAfterDiscounts_EmptyProduct_ReturnsZero()
        {
            Assert.Equal(0, _cart.GetTotalAmountAfterDiscounts());
        }
        [Fact]
        public void GetTotalAmountAfterDiscounts_EmptyDiscount_EqualTotalAmount()
        {
            Category category = new Category("Food");
            Product product = new Product("Apple", 50, category);
            _cart.AddItem(product, 5);

            Assert.Equal(_cart.GetTotalAmount(), _cart.GetTotalAmountAfterDiscounts());
        }

        [Theory]
        [InlineData(DiscountType.Amount)]
        [InlineData(DiscountType.Rate)]
        public void GetTotalAmountAfterDiscounts_WithCampaign_ReturnTrueAmount(DiscountType discountType)
        {
            Category category = new Category("Food");
            Product product = new Product("Apple", 50, category);
            Campaign campaign = new Campaign(category, 4, 100, discountType);

            _cart.AddItem(product, 5);
            _cart.AddCampaings(campaign);

            Assert.Equal(_cart.GetTotalAmount() - _cart.GetCampaignDiscount(), _cart.GetTotalAmountAfterDiscounts());
        }

        [Fact]
        public void GetTotalAmountAfterDiscounts_WithManyCampaigns_ReturnTrueAmount()
        {
            Category category = new Category("Food");
            Product product = new Product("Apple", 50, category);
            Campaign campaign1 = new Campaign(category, 4, 10, DiscountType.Rate);
            Campaign campaign2 = new Campaign(category, 4, 10, DiscountType.Amount);

            _cart.AddItem(product, 5);
            _cart.AddCampaings(campaign1);
            _cart.AddCampaings(campaign2);

            Assert.Equal(_cart.GetTotalAmount() - _cart.GetCampaignDiscount(), _cart.GetTotalAmountAfterDiscounts());
        }

        [Theory]
        [InlineData(DiscountType.Amount, DiscountType.Amount)]
        [InlineData(DiscountType.Amount, DiscountType.Rate)]
        [InlineData(DiscountType.Rate, DiscountType.Amount)]
        [InlineData(DiscountType.Rate, DiscountType.Rate)]
        public void GetTotalAmountAfterDiscounts_WithCampaignAndCoupon_ReturnTrueAmount(DiscountType campaignDiscountType, DiscountType couponDiscountType)
        {
            Category category = new Category("Food");
            Product product = new Product("Apple", 50, category);
            Campaign campaign = new Campaign(category, 4, 10, campaignDiscountType);
            Coupon coupon = new Coupon(4, 10, couponDiscountType);

            _cart.AddItem(product, 5);
            _cart.AddCampaings(campaign);
            _cart.AddCoupons(coupon);

            Assert.Equal((_cart.GetTotalAmount() - _cart.GetCampaignDiscount()) - _cart.GetCouponDiscount(), _cart.GetTotalAmountAfterDiscounts());
        }
        #endregion
        #region GetDeliveryCount
        [Fact]
        public void GetDeliveryCount_EmptyProduct_ReturnsZero()
        {
            Assert.Equal(0, _cart.GetDeliveryCount());
        }
        #endregion
    }
}