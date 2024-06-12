using NUnit.Framework;
using System;

namespace DotRas.Tests {
    [TestFixture]
    public class RasConnectionStatisticsTests {
        [Test]
        public void InitializesTheClassAsExpected() {
            var target = new RasConnectionStatistics(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, TimeSpan.FromMilliseconds(14));

            Assert.Multiple(() => {
                Assert.That(target.BytesTransmitted, Is.EqualTo(1));
                Assert.That(target.BytesReceived, Is.EqualTo(2));
                Assert.That(target.FramesTransmitted, Is.EqualTo(3));
                Assert.That(target.FramesReceived, Is.EqualTo(4));
                Assert.That(target.CrcErrors, Is.EqualTo(5));
                Assert.That(target.TimeoutErrors, Is.EqualTo(6));
                Assert.That(target.AlignmentErrors, Is.EqualTo(7));
                Assert.That(target.HardwareOverrunErrors, Is.EqualTo(8));
                Assert.That(target.FramingErrors, Is.EqualTo(9));
                Assert.That(target.BufferOverrunErrors, Is.EqualTo(10));
                Assert.That(target.CompressionRatioIn, Is.EqualTo(11));
                Assert.That(target.CompressionRatioOut, Is.EqualTo(12));
                Assert.That(target.LinkSpeed, Is.EqualTo(13));
                Assert.That(target.ConnectionDuration.TotalMilliseconds, Is.EqualTo(14));
            });
        }
    }
}
