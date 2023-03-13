
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class StackTests
    {
        [Test]
        public void Push_ArgsIsNull_ReturnArgNullException() 
        {
            var stack = new Stack<int?>();

            Assert.That(() => stack.Push(null), 
                    Throws.ArgumentNullException);
        }

        [Test]
        public void Push_ValidArg_AddTheObjectToTheStack()
        {
            var stack = new Stack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            Assert.That(stack.Count, Is.EqualTo(3));
        }

        [Test]
        public void Count_EmptyStack_ReturnZero()
        {
            var stack = new Stack<int>();

            Assert.That(stack.Count, Is.EqualTo(0));
        }

        [Test]
        public void Pop_EmptyStack_ThrowInvalidOperationException() 
        {
            var stack = new Stack<int>();

            Assert.That(() => stack.Pop(), 
                Throws.InvalidOperationException);
        }

        [Test]
        public void Pop_StackWithAFewObjects_ReturnObjectOnTheTop()
        {
            var stack = new Stack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var result = stack.Pop();

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Pop_StackWithAFewObjects_RemoveObjectOnTheTop()
        {
            var stack = new Stack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            stack.Pop();

            Assert.That(stack.Count, Is.EqualTo(2));
        }

        [Test]
        public void Peek_EmptyStack_ThrowInvalidOperationException()
        {
            var stack = new Stack<int>();

            Assert.That(() => stack.Peek(), 
                Throws.InvalidOperationException);
        }

        [Test]
        public void Peek_StackWithObjects_ReturnObjectsOnTheTopOfTheStack()
        {
            var stack = new Stack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var result = stack.Peek();

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Peek_StackWithObjects_DoesNotRemoveTheObjectOnTopOfTheStack()
        {
            var stack = new Stack<int>();

            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            stack.Peek();

            Assert.That(stack.Count, Is.EqualTo(3));
        }
    }
}
