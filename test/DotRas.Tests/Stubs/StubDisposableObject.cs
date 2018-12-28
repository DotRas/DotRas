namespace DotRas.Tests.Stubs
{
    public class StubDisposableObject : DisposableObject
    {
        public int Counter { get; private set; }

        public new void GuardMustNotBeDisposed()
        {
            base.GuardMustNotBeDisposed();
        }

        protected override void Dispose(bool disposing)
        {
            Counter = Counter + 1;

            base.Dispose(disposing);
        }
    }
}
