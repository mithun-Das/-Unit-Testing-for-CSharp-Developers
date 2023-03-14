
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public void PlaceOrder_WhenCalled_StoreTheOrder()
        {
            var storage = new Mock<IStorage>();
            var OrderService = new OrderService(storage.Object);

            var order = new Order();
            OrderService.PlaceOrder(order);

            storage.Verify(s => s.Store(order));
        }
    }
}
