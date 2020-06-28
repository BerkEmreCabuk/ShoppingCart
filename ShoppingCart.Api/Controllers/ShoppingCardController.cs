using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Api.Domain;
using ShoppingCart.Api.Domain.Enums;
using ShoppingCart.Api.Domain.Interfaces;

namespace ShoppingCart.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ShoppingCartController : ControllerBase
    {

        private readonly ILogger<ShoppingCartController> _logger;
        private ICart _cart;

        public ShoppingCartController(
            ILogger<ShoppingCartController> logger,
            ICart cart)
        {
            _logger = logger;
            _cart = cart;
        }

        [HttpGet("simulation")]
        public IActionResult Simulation()
        {
            #region Instance
            Category food = new Category("Food");
            Category electronic = new Category("Electronic");
            Category smartPhone = new Category("SmartPhone", electronic);

            Product apple = new Product("Apple", 2, food);
            Product banana = new Product("Banana", 4, food);
            Product iPhoneX = new Product("IPhoneX", 5000, smartPhone);

            Campaign foodCampaign = new Campaign(food, 10, 10, DiscountType.Rate);
            Campaign foodCampaign2 = new Campaign(food, 10, 100, DiscountType.Amount);
            Campaign smartPhoneCampaign = new Campaign(smartPhone, 2, 10, DiscountType.Rate);

            Coupon foodCoupon = new Coupon(20, 20, DiscountType.Amount);
            Coupon smartPhoneCoupon = new Coupon(1, 100, DiscountType.Amount);
            #endregion
            #region ShoppingCart add object
            _cart.AddItem(apple, 20.5);
            _logger.LogInformation($"{apple.Title} product added to cart");
            _cart.AddItem(apple, 9.5);
            _logger.LogInformation($"{apple.Title} product added to cart");
            _cart.AddItem(banana, 50);
            _logger.LogInformation($"{banana.Title} product added to cart");
            _cart.AddItem(iPhoneX, 3);
            _logger.LogInformation($"{iPhoneX.Title} product added to cart");
            _cart.AddCampaings(foodCampaign, foodCampaign2, smartPhoneCampaign);
            _cart.AddCoupons(foodCoupon, smartPhoneCoupon);
            #endregion
            return Ok(_cart.Print());
        }
    }
}
