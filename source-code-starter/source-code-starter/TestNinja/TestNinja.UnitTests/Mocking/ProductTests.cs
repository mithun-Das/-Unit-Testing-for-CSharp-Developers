
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void GetPrice_CustomerIsGold_Apply30PercentDiscount()
        {
            var product = new Product { ListPrice = 100 };

            var result = product.GetPrice(new Customer { IsGold = true });

            Assert.That(result, Is.EqualTo(70));
        }

        [Test]
        public void GetPrice_CustomerIsGold_Apply30PercentDiscountWithMock()
        {
            var customer = new Mock<ICustomer>();
            var product = new Product { ListPrice = 100 };

            customer.Setup(x => x.IsGold).Returns(true);
            //customer.Setup(x => x.IsGold).Returns(false);

            var result = product.GetPrice(customer.Object);

            Assert.That(result, Is.EqualTo(70));
        }
    }
}
