using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests{
    public class OrderControllerTests{
        [Fact]
        public void Cannot_Checkout_Empty_Cart(){
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            Order order = new Order();
            OrderController target = new OrderController(mock.Object, cart);

            // Act
            ViewResult result = target.Checkout(order) as ViewResult;

            // Assert
            //check that the order hasn't been stored
            mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // check method is returning default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // check that the view is invalid
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails() {
            // Arrange
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //Create instance of the controller
            OrderController target = new OrderController(mock.Object, cart);
            // Add error to the model
            target.ModelState.AddModelError("error", "error");
            
            // Act - try to checkout
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // Assert
            //check that the order hasn't been passed stored
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            //Check the method is returning the default view
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            //Check that I am passing an invalid model to the view
            Assert.False(result.ViewData.ModelState.IsValid);
        }
        [Fact]
        public void Can_Checkout_And_Submit_Order(){
            //arrange create mock order repository
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(),1);
            OrderController target = new OrderController(mock.Object, cart);

            //act
            RedirectToPageResult result = target.Checkout(new Order()) as RedirectToPageResult;

            //assert
            mock.Verify(m=>m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal("/Completed", result.PageName);
        }
    }
}