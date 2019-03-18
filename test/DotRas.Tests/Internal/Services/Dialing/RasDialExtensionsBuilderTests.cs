using System;
using System.Windows.Forms;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Services.Dialing;
using Moq;
using NUnit.Framework;
using static DotRas.Internal.Interop.Ras;

namespace DotRas.Tests.Internal.Services.Dialing
{
    [TestFixture]
    public class RasDialExtensionsBuilderTests
    {
        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new RasDialExtensionsBuilder(null));
        }

        [Test]
        public void ConfiguresTheOwnerHandleAsExpected()
        {
            var expected = new IntPtr(1);

            var factory = new Mock<IStructFactory>();

            var owner = new Mock<IWin32Window>();
            owner.Setup(o => o.Handle).Returns(expected);

            var target = new RasDialExtensionsBuilder(factory.Object);
            var result = target.Build(new RasDialContext
            {
               Options = new RasDialerOptions
               {
                   Owner = owner.Object
               }
            });

            Assert.AreEqual(expected, result.hwndParent);
        }

        [Test]
        public void ConfiguresNoOptionsByDefault()
        {
            var factory = new Mock<IStructFactory>();

            var target = new RasDialExtensionsBuilder(factory.Object);
            var result = target.Build(new RasDialContext());

            Assert.AreEqual(RDEOPT.None, result.dwfOptions);
        }
    }
}