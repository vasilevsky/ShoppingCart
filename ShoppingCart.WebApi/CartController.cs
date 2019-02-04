﻿using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.WebApi
{
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


        [HttpGet("{cartId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetCart([FromRoute]Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("No cart id specified");

            var cart = cartRepository.GetCart(cartId.Value);

            return Ok(cart);
        }

        [HttpPost("{cartId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult AddToCart([FromRoute]Guid cartId, [FromBody]ItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var result = cartService.AddItem(cartId, itemData);

            return HandleResult(result, NoContent());
        }

        [HttpPatch("{cartId:Guid}/items")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateQuantity(
            [FromRoute]Guid cartId,
            [FromBody]ItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var result = cartService.UpdateItem(cartId, itemData.ProductId, itemData.Quantity);

            return HandleResult(result, NoContent());
        }

        [HttpPost()]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult CreateCart([FromBody]ItemData itemData)
        {
            if (!ModelState.IsValid)
                return BadRequest("Item data is invalid");

            var result = cartService.CreateCart(itemData);

            return HandleResult(result);
        }

        [HttpDelete("{cartId:Guid}/items")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult ClearCart([FromRoute]Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("No cart id specified");

            var result = cartService.ClearCart(cartId.Value);

            return HandleResult(result, NoContent());
        }

        [HttpDelete("{cartId:Guid}/items/{productId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult DeleteItem([FromRoute]Guid? cartId, [FromRoute]int? productId)
        {
            if (cartId == null || productId == null)
                return BadRequest();

            var result = cartService.DeleteCartItem(cartId.Value, productId.Value);

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
