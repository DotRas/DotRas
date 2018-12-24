using static DotRas.Internal.Interop.Ras;

namespace DotRas
{
    /// <summary>
    /// Defines the states for Internet Key Exchange version 2 (IKEv2) virtual private network (VPN) tunnel connections.
    /// </summary>
    /// <remarks>These states are not available to other tunneling protocols.</remarks>
    public enum RasConnectionSubState
    {
        /// <summary>
        /// The connection state does not have a sub-state.
        /// </summary>
        None = 0,

        /// <summary>
        /// The underlying internet interface of the connection is down and the connection is waiting for an internet interface to come online.
        /// </summary>
        Dormant,

        /// <summary>
        /// The internet interface has come online and the connection is switching over to this new interface through Mobile IKE (MOBIKE) update.
        /// </summary>
        Reconnecting,

        /// <summary>
        /// The Mobile IKE (MOBIKE) update has completed and the connection has switched over successfully to the new internet interface.
        /// </summary>
        Reconnected = RASCSS_DONE
    }
}