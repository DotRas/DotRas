using System;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasHandleTests
    {
        [Test]
        public void HashCodeMustBeTheSameBetweenInstances()
        {
            var handle1 = RasHandle.FromPtr(new IntPtr(1));
            var handle2 = RasHandle.FromPtr(new IntPtr(1));

            Assert.AreEqual(handle1.GetHashCode(), handle2.GetHashCode());
        }

        [Test]
        public void HashCodeMustBeDifferenceBetweenInstances()
        {
            var handle1 = RasHandle.FromPtr(new IntPtr(1));
            var handle2 = RasHandle.FromPtr(new IntPtr(2));

            Assert.AreNotEqual(handle1.GetHashCode(), handle2.GetHashCode());
        }

        [Test]
        public void ThrowsAnExceptionWhenTheValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => RasHandle.FromPtr(IntPtr.Zero));
        }

        [Test]
        public void TwoHandlesWithSamePointerMustBeEqual()
        {
            var handle1 = RasHandle.FromPtr(new IntPtr(1));
            var handle2 = RasHandle.FromPtr(new IntPtr(1));

            Assert.AreEqual(handle1, handle2);
        }

        [Test]
        public void TwoHandlesWithSamePointerMustBeEqualWhenUsingTheEqualityOperator()
        {
            var handle1 = RasHandle.FromPtr(new IntPtr(1));
            var handle2 = RasHandle.FromPtr(new IntPtr(1));

            Assert.IsTrue(handle1 == handle2);
        }

        [Test]
        public void MustNotEqualNullWhenLeftOperand()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));

            Assert.IsFalse(handle == null);
        }

        [Test]
        public void MustNotEqualNullWhenRightOperand()
        {
            var handle = RasHandle.FromPtr(new IntPtr(1));

            Assert.IsFalse(null == handle);
        }

        [Test]
        public void TwoNullInstancesShouldBeEqual()
        {
            RasHandle handle1 = null;
            RasHandle handle2 = null;

            Assert.IsTrue(handle1 == handle2);
        }

        [Test]
        public void TheObjectShouldBeEqualToTheHandle()
        {
            object handle1 = RasHandle.FromPtr(new IntPtr(1));
            var handle2 = RasHandle.FromPtr(new IntPtr(1));

            Assert.IsTrue(handle2.Equals(handle1));
        }
    }
}