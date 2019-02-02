using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.WebApi
{
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly CartService _cartService;

        public CartController(ICartRepository cartRepository, CartService cartService)
        {
            _cartRepository = cartRepository;
            _cartService = cartService;
        }


        [HttpGet("{cartId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCart([FromRoute]Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("No cart id specified");

            var cart = _cartRepository.GetCart(cartId.Value);

            return Ok(cart);
        }

        [HttpPost("{cartId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddToCart([FromRoute]Guid cartId, [FromBody]AddItemData itemData)
        {
            if (!ModelState.IsValid || cartId == null)
                return BadRequest("Item data is invalid");

            var id = _cartService.AddItem(cartId, itemData);

            return Ok(id);
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateCart([FromBody]AddItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var id = _cartService.CreateCart(itemData);

            return Created("api/card", id);
        }

        [HttpDelete("{cartId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ClearCart([FromRoute]Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("Item data is invalid");

            var cart = _cartRepository.GetCart(cartId.Value);
            cart.ClearAllItems();

            return Ok();
        }

        [HttpDelete("{cartId:Guid}/items/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteItem(Guid? cartId, int? productId)
        {
            if (cartId == null || productId == null)
                return BadRequest();

            var cart = _cartRepository.GetCart(cartId.Value);
            cart.DeleteItemBy(productId.Value);

            return Ok();
        }
    }
}
