using System.Reflection;
using GrubifyApi.Controllers;
using GrubifyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrubifyApi.Tests;

public class CartControllerTests
{
    [Fact]
    public void AddItemToCart_DoesNotRetainRequestBuffers()
    {
        var controller = new CartController();
        var userId = $"test-{Guid.NewGuid():N}";

        for (var i = 0; i < 100; i++)
        {
            var result = controller.AddItemToCart(userId, new AddCartItemRequest
            {
                FoodItemId = 1,
                Quantity = 1
            });

            var response = Assert.IsType<OkObjectResult>(result.Result);
            var cart = Assert.IsType<Cart>(response.Value);
            Assert.Single(cart.Items);
        }

        Assert.Null(typeof(CartController).GetField(
            "RequestDataCache",
            BindingFlags.Static | BindingFlags.NonPublic));
    }
}
