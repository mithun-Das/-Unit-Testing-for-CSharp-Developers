

using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class EmployeeControllerTests
    {
        private Mock<IEmployeeStorage> _storage;
        private EmployeeController _controller;

        [SetUp] 
        public void SetUp() 
        {
            _storage = new Mock<IEmployeeStorage>();
            _controller = new EmployeeController(_storage.Object);
        }

        [Test]
        public void DeleteEmployee_StorageCheck_ReturnActionResult()
        {
            var result = _controller.DeleteEmployee(1);

            _storage.Verify(x => x.DeleteEmployee(1));
        }

        [Test]
        public void DeleteEmployee_FunctionCheck_ReturnActionResult()
        {
            var result = _controller.DeleteEmployee(1);

            Assert.That(result, Is.TypeOf<RedirectResult>());
        }
    }
}
