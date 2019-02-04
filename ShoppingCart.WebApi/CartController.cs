using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ShoppingCart.WebApi
{
    [Description("Provides functionality for managing shopping cart")]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly CartService cartService;

        public CartController(ICartRepository cartRepository, CartService cartService)
        {
            this.cartRepository = cartRepository;
            this.cartService = cartService;
        }

        [Description("Gets cart items of specified cart.")]
        [HttpGet("{cartId:Guid}")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetCart(
            [Description("Id of the cart.")]
            [FromRoute]Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("No cart id specified");

            var cart = cartRepository.GetCart(cartId.Value);

            return Ok(cart);
        }

        [Description("Add cart item to specified cart.")]
        [HttpPost("{cartId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult AddToCart(
            [Description("Id of the cart.")]
            [FromRoute]Guid cartId,
            [Description("Cart item data.")]
            [FromBody]ItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var result = cartService.AddItem(cartId, itemData);

            return HandleResult(result, NoContent());
        }

        [Description("Update quantity of specific product in the specified cart.")]
        [HttpPatch("{cartId:Guid}/items")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateQuantity(
            [Description("Id of the cart.")]
            [FromRoute]Guid cartId,
            [Description("Cart item data to be updated.")]
            [FromBody]ItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var result = cartService.UpdateItem(cartId, itemData.ProductId, itemData.Quantity);

            return HandleResult(result, NoContent());
        }

        [Description("Creates new cart and returns Id of it.")]
        [HttpPost()]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CreateCart(
            [Description("Cart item data to be added to the new cart.")]
            [FromBody]ItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var result = cartService.CreateCart(itemData);

            return HandleResult(result);
        }

        [Description("Removes all cart items.")]
        [HttpDelete("{cartId:Guid}/items")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public IActionResult ClearCart(
            [Description("Id of the cart.")]
            [FromRoute]Guid cartId)
        {
            var result = cartService.ClearCart(cartId);

            return HandleResult(result, NoContent());
        }

        [Description("Removes specified item of cart.")]
        [HttpDelete("{cartId:Guid}/items/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult DeleteItem(
            [Description("Id of the cart.")]
            [FromRoute]Guid cartId,
            [Description("Id of product to be removed.")]
            [FromRoute]int productId)
        {
            var result = cartService.DeleteCartItem(cartId, productId);

            return HandleResult(result, NoContent());
        }

        private IActionResult HandleResult<TUnit, TFailure>(Either<TUnit, TFailure> result, IActionResult onSuccess = null)
            where TFailure : Failure
        {
            return result.Failure == null
                ? onSuccess ?? Ok(result.Unit)
                : HandleFailure(result.Failure);
        }

        private IActionResult HandleFailure(Failure failure)
        {
            switch (failure)
            {
                case NotFound notFound: return NotFound();
                default: throw new Exception("Not supported Failure");
            }
        }
    }
}
