using System;
using NUnit.Framework;

namespace DotRas.Tests
{
    [TestFixture]
    public class RasConnectionStatisticsTests
    {
        [Test]
        public void InitializesTheClassAsExpected()
        {
            var target = new RasConnectionStatistics(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, TimeSpan.FromMilliseconds(14));

            Assert.AreEqual(1, target.BytesTransmitted);
            Assert.AreEqual(2, target.BytesReceived);
            Assert.AreEqual(3, target.FramesTransmitted);
            Assert.AreEqual(4, target.FramesReceived);
            Assert.AreEqual(5, target.CrcError);
            Assert.AreEqual(6, target.TimeoutError);
            Assert.AreEqual(7, target.AlignmentError);
            Assert.AreEqual(8, target.HardwareOverrunError);
            Assert.AreEqual(9, target.FramingError);
            Assert.AreEqual(10, target.BufferOverrunError);
            Assert.AreEqual(11, target.CompressionRatioIn);
            Assert.AreEqual(12, target.CompressionRatioOut);
            Assert.AreEqual(13, target.LinkSpeed);
            Assert.AreEqual(14, target.ConnectionDuration.TotalMilliseconds);
        }
    }
}