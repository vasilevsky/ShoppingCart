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

namespace ShoppingCart.WebApi.Tests.Integration
{
    public class CartTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private Mock<ICartIdGenerator> mock = new Mock<ICartIdGenerator>();
        private readonly WebApplicationFactory<Startup> _fixture;

        public CartTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(
                    services =>
                    {
                        services.AddSingleton<ICartIdGenerator>(mock.Object);
                    });
            });
        }

        [Fact]
        public async Task Creates_new_cart_with_item_and_returns_cart_id()
        {
            var newCartId = Guid.NewGuid();
            mock.Setup(m => m.NewId()).Returns(newCartId);

            var client = _fixture.CreateClient();
            var response = await client.PostAsJsonAsync($"/api/cart",
                new AddItemData()
                {
                    ProductId = 7
                });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe(newCartId.ToString());
        }

        [Fact]
        public async Task Adds_item_to_cart()
        {
            var cartId = Guid.NewGuid();

            var client = _fixture.CreateClient();
            var response = await client.PostAsJsonAsync($"/api/cart/{cartId}",
                new AddItemData()
                {
                    ProductId = 7
                });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
