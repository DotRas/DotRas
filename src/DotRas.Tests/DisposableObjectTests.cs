using System;
using DotRas.Tests.Stubs;
using NUnit.Framework;

#pragma warning disable S3966 // This is intentional to dispose multiple times to check the behavior.

namespace DotRas.Tests
{
    [TestFixture]
    public class DisposableObjectTests
    {
        [Test]
        public void DoesNotThrowAnExceptionWhenNotDisposed()
        {
            var target = new StubDisposableObject();
            Assert.DoesNotThrow(() => target.GuardMustNotBeDisposed());
        }

        [Test]
        public void ThrowsAnExceptionWhenDisposed()
        {
            var target = new StubDisposableObject();
            target.Dispose();

            var ex = Assert.Throws<ObjectDisposedException>(() => target.GuardMustNotBeDisposed());
            Assert.AreEqual(typeof(StubDisposableObject).FullName, ex.ObjectName);
        }

        [Test]
        public void DoesNotErrorWhenDisposedMoreThanOnce()
        {
            var target = new StubDisposableObject();
            
            Assert.DoesNotThrow(() => target.Dispose());
            Assert.AreEqual(1, target.Counter);

            Assert.DoesNotThrow(() => target.Dispose());
            Assert.AreEqual(2, target.Counter);
        }
    }
}