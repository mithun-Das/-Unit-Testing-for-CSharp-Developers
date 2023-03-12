
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        [TestCase(15)]
        [TestCase(30)]
        public void GetOutput_InputIsDivisibleBy3And5_ReturnFizzBuzz(int number) 
        {
            var result = FizzBuzz.GetOutput(number);

            Assert.That(result, Is.EqualTo("FizzBuzz"));
        }

        [Test]
        [TestCase(12)]
        [TestCase(24)]
        public void GetOutput_InputIsDivisibleBy3_ReturnFizz(int number) 
        {
            var result = FizzBuzz.GetOutput(number);

            Assert.That(result, Is.EqualTo("Fizz"));
        }

        [Test]
        [TestCase(20)]
        public void GetOutput_InputIsDivisibleBy5_ReturnBuzz(int number) 
        {
            var result = FizzBuzz.GetOutput(number);

            Assert.That(result, Is.EqualTo("Buzz"));
        }

        [Test]
        [TestCase(4)]
        public void GetOutput_InputIsNotDivisibleBy3Or5_ReturnTheSameNumber(int number) 
        {
            var result = FizzBuzz.GetOutput(number);

            Assert.That(result, Is.EqualTo(number.ToString()));
        }
    }
}
