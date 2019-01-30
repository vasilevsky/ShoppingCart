using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace ShoppingCart.WebApi.Tests.Integration
{
    public class CartTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _fixture;

        public CartTests(WebApplicationFactory<Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Creates_new_cart_and_returns_cart_id()
        {
            var client = _fixture.CreateClient();
            var cartId = Guid.NewGuid();
            var response = await client.PostAsJsonAsync($"/api/cart/{cartId}",
                new AddItemData()
                {
                    ProductId = 7
                });

            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldBe("asd");
        }
    }
}
