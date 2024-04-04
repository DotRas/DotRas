﻿using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Tests.Stubs;
using NUnit.Framework;

namespace DotRas.Tests.Diagnostics;

[TestFixture]
public class ConventionBasedEventFormatterFactoryTests
{
    [Test]
    public void ThrowsAnExceptionWhenTheAttributeDoesNotExist()
    {
        var target = new ConventionBasedEventFormatterFactory();
        Assert.Throws<FormatterNotFoundException>(() => target.Create<TraceEvent>());
    }

    [Test]
    public void ThrowsAnExceptionWhenTheFormatterIsTheWrongType()
    {
        var target = new ConventionBasedEventFormatterFactory();
        Assert.Throws<InvalidOperationException>(() => target.Create<BadTraceEvent>());
    }

    [Test]
    public void ThrowsAnExceptionWhenTheFormatterCannotBeCreated()
    {
        var target = new ConventionBasedEventFormatterFactory();
        Assert.Throws<FormatterNotFoundException>(() => target.Create<BadTraceEventWithBadFormatter>());
    }

    [Test]
    public void ReturnsTheFormatterAsExpected()
    {
        var target = new ConventionBasedEventFormatterFactory();

        var formatter = target.Create<GoodTraceEventWithGoodFormatter>();
        Assert.IsInstanceOf<GoodFormatter>(formatter);
    }
}