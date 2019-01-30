using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.WebApi
{
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{cartId:Guid}")]
        public async Task<IActionResult> AddToCart(Guid cartId)
        {
            return Ok("asd");
        }
    }
}
