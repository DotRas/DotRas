using System;
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
        private Mock<IStructFactory> factory;
        private Mock<IRasGetEapUserData> getEapUserData;
        private Mock<IMarshaller> marshaller;

        [SetUp]
        public void Init()
        {
            factory = new Mock<IStructFactory>();
            getEapUserData = new Mock<IRasGetEapUserData>();
            marshaller = new Mock<IMarshaller>();
        }

        [Test]
        public void ThrowsAnExceptionWhenStructFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new RasDialExtensionsBuilder(null, getEapUserData.Object, marshaller.Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenGetEapUserDataIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new RasDialExtensionsBuilder(factory.Object, null, marshaller.Object));
        }

        [Test]
        public void ThrowsAnExceptionWhenContextIsNull()
        {
            var target = new RasDialExtensionsBuilder(factory.Object, getEapUserData.Object, marshaller.Object);
            Assert.Throws<ArgumentNullException>(() => target.Build(null));
        }

        [Test]
        public void ConfiguresTheOwnerHandleAsExpected()
        {
            var expected = new IntPtr(1);

            var target = new RasDialExtensionsBuilder(factory.Object, getEapUserData.Object, marshaller.Object);
            var result = target.Build(new RasDialContext
            {
               Options = new RasDialerOptions
               {
                   Owner = expected
               }
            });

            Assert.AreEqual(expected, result.hwndParent);
        }

        [Test]
        public void ConfiguresNoOptionsByDefault()
        {
            var target = new RasDialExtensionsBuilder(factory.Object, getEapUserData.Object, marshaller.Object);
            var result = target.Build(new RasDialContext());

            Assert.AreEqual(RDEOPT.None, result.dwfOptions);
        }
    }
}