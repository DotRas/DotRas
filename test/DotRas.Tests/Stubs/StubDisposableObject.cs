namespace DotRas.Tests.Stubs
{
    public class StubDisposableObject : DisposableObject
    {
        public new void GuardMustNotBeDisposed()
        {
            base.GuardMustNotBeDisposed();
        }
    }
}
