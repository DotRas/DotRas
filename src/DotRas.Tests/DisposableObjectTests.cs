using DotRas.Tests.Stubs;
using NUnit.Framework;
using System;

namespace DotRas.Tests {
    [TestFixture]
    public class DisposableObjectTests {
        [Test]
        public void DoesNotThrowAnExceptionWhenNotDisposed() {
            var target = new StubDisposableObject();
            Assert.DoesNotThrow(() => target.GuardMustNotBeDisposed());
        }

        [Test]
        public void ThrowsAnExceptionWhenDisposed() {
            var target = new StubDisposableObject();
            target.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() => target.GuardMustNotBeDisposed());
            Assert.That(ex.ObjectName, Is.EqualTo(typeof(StubDisposableObject).FullName));
        }

        [Test]
        public void DoesNotDisposeTheObjectMoreThanOnce() {
            var target = new StubDisposableObject();
            target.Dispose();

            Assert.That(target.Counter, Is.EqualTo(1));

            target.Dispose();

            Assert.That(target.Counter, Is.EqualTo(1));
        }
    }
}
