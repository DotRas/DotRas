//using System;
//using DotRas.Diagnostics;
//using Moq;
//using NUnit.Framework;

//namespace DotRas.Tests.Diagnostics
//{
//    [TestFixture]
//    public class DependencyResolverTests
//    {
//        private Mock<ILog> log;

//        [SetUp]
//        public void Init()
//        {
//            log = new Mock<ILog>();
//        }

//        [TearDown]
//        public void Complete()
//        {
//            DependencyResolver.Clear();
//        }

//        [Test]
//        public void ThrowsAnExceptionWhenValueIsNull()
//        {
//            Assert.Throws<ArgumentNullException>(() => DependencyResolver.SetLocator(null));
//        }

//        [Test]
//        public void ReturnsTheLoggerAsExpected()
//        {
//            DependencyResolver.SetLocator(() => log.Object);

//            var actual = DependencyResolver.Current;

//            Assert.AreSame(log.Object, actual);
//        }

//        [Test]
//        public void ReturnsNullWhenTheLoggerIsNotSet()
//        {
//            var actual = DependencyResolver.Current;

//            Assert.IsNull(actual);
//        }
//    }
//}