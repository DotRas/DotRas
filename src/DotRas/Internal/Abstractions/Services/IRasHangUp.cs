﻿namespace DotRas.Internal.Abstractions.Services;

internal interface IRasHangUp
{
    Task HangUpAsync(IRasConnection connection, bool closeAllReferences, CancellationToken cancellationToken);

    void UnsafeHangUp(IntPtr handle, bool closeAllReferences, CancellationToken cancellationToken);
}