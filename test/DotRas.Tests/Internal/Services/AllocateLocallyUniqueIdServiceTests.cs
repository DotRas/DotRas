using System.ComponentModel;
using DotRas.Internal.Interop;
using DotRas.Internal.Services;
using Moq;
using NUnit.Framework;

namespace DotRas.Tests.Internal.Services
{
    [TestFixture]
    public class AllocateLocallyUniqueIdServiceTests
    {
        private delegate bool AllocateLocallyUniqueIdCallback(
            out Luid luid);

        [Test]
        public void ReturnsTheResultAsExpected()
        {
            var expected = new Luid(1, 1);

            var api = new Mock<IAdvApi32>();
            api.Setup(o => o.AllocateLocallyUniqueId(out It.Ref<Luid>.IsAny)).Returns(new AllocateLocallyUniqueIdCallback(
                (out Luid luid) =>
                {
                    luid = expected;
                    return true;
                }));

            var target = new AllocateLocallyUniqueIdService(api.Object);
            var actual = target.AllocateLocallyUniqueId();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ThrowsAnExceptionWhenTheResultIsNonSuccess()
        {
            var api = new Mock<IAdvApi32>();
            api.Setup(o => o.AllocateLocallyUniqueId(out It.Ref<Luid>.IsAny)).Returns(new AllocateLocallyUniqueIdCallback(
                (out Luid luid) =>
                {
                    luid = Luid.Empty;
                    return false;
                }));

            var target = new AllocateLocallyUniqueIdService(api.Object);
            Assert.Throws<Win32Exception>(() => target.AllocateLocallyUniqueId());
        }
    }
}