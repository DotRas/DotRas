using static DotRas.Internal.Interop.Ras;

namespace DotRas
{
    /// <summary>
    /// Defines the different sub-states available for a remote access service (RAS) connection.
    /// </summary>
    public enum RasConnectionSubState
    {
        None = 0,
        Dormant,
        Reconnecting,
        Reconnected = RASCSS_DONE
    }
}