using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;
using Moq;
using NUnit.Framework;
using System;

namespace DotRas.Tests {
    [TestFixture]
    public class LuidTests {
        private Mock<IServiceProvider> container;

        [SetUp]
        public void Setup() {
            container = new Mock<IServiceProvider>();
            ServiceLocator.SetLocator(() => container.Object);
        }

        [TearDown]
        public void TearDown() => ServiceLocator.Reset();

        [Test]
        public void AllocatesTheLuidAsExpected() {
            var expected = new Luid(1, 1);

            var allocateLocallyUniqueId = new Mock<IAllocateLocallyUniqueId>();
            allocateLocallyUniqueId.Setup(o => o.AllocateLocallyUniqueId()).Returns(expected);

            container.Setup(o => o.GetService(typeof(IAllocateLocallyUniqueId))).Returns(allocateLocallyUniqueId.Object);

            var actual = Luid.NewLuid();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void EmptyEqualsItselfAsExpected() {
            var _luid = Luid.Empty;
            Assert.That(_luid, Is.EqualTo(expected: Luid.Empty));
        }

        [Test]
        public void ReturnsFalseWhenTwoValuesAreNotEqual() {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.That(target1, Is.Not.EqualTo(target2));
        }

        [Test]
        public void ReturnsFalseWhenTwoValuesAreNotEqualWithYodaSyntax() {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.That(target2, Is.Not.EqualTo(target1));
        }

        [Test]
        public void ReturnsTrueWhenTwoValuesAreNotEqual() {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.That(target1, Is.Not.EqualTo(target2));
        }

        [Test]
        public void ReturnsTrueWhenTwoValuesAreNotEqualWithYodaSyntax() {
            var target1 = new Luid(0, 0);
            var target2 = new Luid(1, 1);

            Assert.That(target2, Is.Not.EqualTo(target1));
        }

        [Test]
        public void ReturnsFalseWhenTheObjectIsNull() {
            var target = new Luid(0, 0);
            var result = target.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ReturnsTrueWhenTheValuesAreEqualWhenTargetIsBoxed() {
            var target1 = new Luid(1, 1);
            object target2 = new Luid(1, 1);

            var result = target1.Equals(target2);
            Assert.That(result, Is.True);
        }

        [Test]
        public void ReturnsFalseWhenTheValuesAreNotEqualWhenTargetIsBoxed() {
            var target1 = new Luid(1, 1);
            object target2 = new Luid(2, 2);

            var result = target1.Equals(target2);
            Assert.That(result, Is.False);
        }

        [Test]
        public void ReturnsTheStringAsExpected() {
            var target = new Luid(1, 2);
            Assert.That(target.ToString(), Is.EqualTo("8589934593"));
        }

        [Test]
        public void ReturnsTheInt64AsExpected() {
            var target = new Luid(1, 2);
            Assert.That(target.ToInt64(), Is.EqualTo(8589934593));
        }

        [Test]
        public void ReturnsTheHashCodeAsExpected() {
            var target = new Luid(1, 2);
            Assert.That(target.GetHashCode(), Is.EqualTo(3));
        }
    }
}
