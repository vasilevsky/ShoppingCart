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
        public async Task<IActionResult> GetCart(Guid? cartId)
        {
            if (cartId == null)
                return BadRequest("No cart id specified");

            var cart = _cartRepository.GetCart(cartId.Value);

            return Ok(cart);
        }

        [HttpPost("{cartId:Guid}")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddToCart(Guid? cartId, [FromBody]AddItemData itemData)
        {
            if (!ModelState.IsValid || cartId == null)
                return BadRequest("Cart ID or Item data is invalid");

            var id = _cartService.CreateOrAddtoExisting(cartId.Value, itemData);

            return Ok(id);
        }

        [HttpDelete("{cartId:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ClearCart(Guid? cartId)
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
    
    public interface ICartFactory
    {
        Cart GetOrCreate(Guid cartId);
    }

    public class CartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Either<Guid, Failure> CreateOrAddtoExisting(Guid cartId, AddItemData itemData)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
            {
                cart = new Cart();
                _cartRepository.Add(cart);
            }

            cart.Add(itemData.ProductId);
            _cartRepository.Save(cart);

            return cartId;
        }
    }

    public class Either<TUnit, TFailure>
        where TFailure : Failure
    {
        public TUnit Unit { get; }

        public TFailure Failure { get; }

        public Either(TFailure failure)
        {
            Failure = failure;
        }

        public Either(TUnit result)
        {
            Unit = result;
        }

        public static implicit operator Either<TUnit, TFailure>(TUnit result)
        {
            return new Either<TUnit, TFailure>(result);
        }

        public static implicit operator Either<TUnit, TFailure>(TFailure failure)
        {
            return new Either<TUnit, TFailure>(failure);
        }
    }

    public abstract class Failure
    {
    }
}
