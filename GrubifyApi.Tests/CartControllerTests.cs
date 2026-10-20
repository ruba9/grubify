using GrubifyApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GrubifyApi.Tests;

public class CartControllerTests
{
    [Fact]
    public void AddItemToCart_DoesNotRetainLargeRequestBuffers()
    {
        var controller = new CartController();
        var userId = Guid.NewGuid().ToString();
        var before = GC.GetTotalMemory(forceFullCollection: true);

        for (var i = 0; i < 12; i++)
        {
            var result = controller.AddItemToCart(userId, new AddCartItemRequest { FoodItemId = 1, Quantity = 1 });
            Assert.IsType<OkObjectResult>(result.Result);
        }

        var after = GC.GetTotalMemory(forceFullCollection: true);
        Assert.True(after - before < 20 * 1024 * 1024,
            $"Cart additions retained {after - before} bytes; expected less than 20 MiB.");
    }
}
