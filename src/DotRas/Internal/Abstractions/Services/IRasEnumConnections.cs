﻿namespace DotRas.Internal.Abstractions.Services;

internal interface IRasEnumConnections
{
    IEnumerable<RasConnection> EnumerateConnections();
}