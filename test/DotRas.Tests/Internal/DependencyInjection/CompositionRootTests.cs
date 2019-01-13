using DotRas.Internal.Composition;
using DotRas.Internal.Interop;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Composition
{
    [TestFixture]
    public class CompositionRootTests
    {
        [Test]
        public void ShouldReturnDifferentObjectsPerCall()
        {
            var target = new CompositionRoot();
            target.RegisterCallback(c => new Mock<IRasApi32>().Object);

            var call1 = target.GetService(typeof(IRasApi32));
            var call2 = target.GetService(typeof(IRasApi32));

            Assert.AreNotSame(call1, call2);
        }
    }
}