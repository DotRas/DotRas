using System;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class LuidTests
    {
        private Mock<IServiceProvider> container;

        [SetUp]
        public void Setup()
        {
            container = new Mock<IServiceProvider>();
            CompositionRoot.Default = container.Object;
        }

        [TearDown]
        public void TearDown()
        {
            CompositionRoot.Clear();
        }

        [Test]
        public void AllocatesTheLuidAsExpected()
        {
            var expected = new Luid(1, 1);

            var allocateLocallyUniqueId = new Mock<IAllocateLocallyUniqueId>();
            allocateLocallyUniqueId.Setup(o => o.AllocateLocallyUniqueId()).Returns(expected);

            container.Setup(o => o.GetService(typeof(IAllocateLocallyUniqueId))).Returns(allocateLocallyUniqueId.Object);

            var actual = Luid.NewLuid();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EmptyEqualsItselfAsExpected()
        {
            Assert.AreEqual(Luid.Empty, Luid.Empty);
        }

        [Test]
        public void ReturnsFalseWhenTwoValuesAreNotEqual()
        {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.False(target1 == target2);
        }

        [Test]
        public void ReturnsFalseWhenTwoValuesAreNotEqualWithYodaSyntax()
        {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.False(target2 == target1);
        }

        [Test]
        public void ReturnsTrueWhenTwoValuesAreNotEqual()
        {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.True(target1 != target2);
        }

        [Test]
        public void ReturnsTrueWhenTwoValuesAreNotEqualWithYodaSyntax()
        {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.True(target2 != target1);
        }

        [Test]
        public void ReturnsFalseWhenTheObjectIsNull()
        {
            var target = new Luid(0, 0);
            var result = target.Equals(null);

            Assert.False(result);
        }

        [Test]
        public void ReturnsTrueWhenTheValuesAreEqualWhenTargetIsBoxed()
        {
            var target1 = new Luid(1, 1);
            object target2 = new Luid(1, 1);

            var result = target1.Equals(target2);
            Assert.True(result);
        }

        [Test]
        public void ReturnsFalseWhenTheValuesAreNotEqualWhenTargetIsBoxed()
        {
            var target1 = new Luid(1, 1);
            object target2 = new Luid(2, 2);

            var result = target1.Equals(target2);
            Assert.False(result);
        }

        [Test]
        public void ReturnsTheStringAsExpected()
        {
            var target = new Luid(1, 2);
            Assert.AreEqual("8589934593", target.ToString());
        }

        [Test]
        public void ReturnsTheInt64AsExpected()
        {
            var target = new Luid(1, 2);
            Assert.AreEqual(8589934593, target.ToInt64());
        }

        [Test]
        public void ReturnsTheHashCodeAsExpected()
        {
            var target = new Luid(1, 2);
            Assert.AreEqual(3, target.GetHashCode());
        }
    }
}