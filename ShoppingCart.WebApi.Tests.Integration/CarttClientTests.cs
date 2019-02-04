using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;
using System.Linq;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Headers;
using ShoppingCart.WebApi.Client;

namespace ShoppingCart.WebApi.Tests.Integration
{
    public class CartClientTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private Mock<ICartIdGenerator> mock = new Mock<ICartIdGenerator>();
        private readonly WebApplicationFactory<Startup> fixture;

        public CartClientTests(WebApplicationFactory<Startup> fixture)
        {
            this.fixture = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(
                    services =>
                    {
                        services.AddSingleton(mock.Object);
                    });
            });
        }

        [Fact]
        public async Task Creates_cart_and_manipulates_items()
        {
            var client = CartClient(fixture.CreateClient());

            var newCartId = Guid.NewGuid();
            mock.Setup(m => m.NewId()).Returns(newCartId);

            var cartId = await client.CreateCart(new Client.ItemData() { ProductId = 7, Quantity = 1 });

            cartId.ShouldBe(newCartId);

            await client.AddToCart(cartId, new Client.ItemData() { ProductId = 8, Quantity = 2 });

            await client.DeleteItems(cartId, 7);
        }

        [Fact]
        public async Task Creates_cart_andg_manipulates_items()
        {
            var client = CartClient(fixture.CreateClient());

            var newCartId = Guid.NewGuid();
            mock.Setup(m => m.NewId()).Returns(newCartId);

            var cartId = await client.CreateCart(new Client.ItemData() { ProductId = 7, Quantity = 1 });

            cartId.ShouldBe(newCartId);

            await client.AddToCart(cartId, new Client.ItemData() { ProductId = 8, Quantity = 2 });
            await client.DeleteItems(cartId, 7);
        }


        private CartClient CartClient(HttpClient httpClient)
            => new CartClient(
                    new CartClientConfiguration()
                        .SetApiKey("Chuck Norris")
                        .SetEndpointAddress("/api"),
                    httpClient);
    }
}
