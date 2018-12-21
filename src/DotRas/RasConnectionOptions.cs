using static DotRas.Internal.Interop.Ras;

namespace DotRas
{
    /// <summary>
    /// Represents connection options for a remote access service (RAS) connection.
    /// </summary>
    public class RasConnectionOptions
    {
        private readonly RASCF flags;

        internal RasConnectionOptions(RASCF flags)
        {
            this.flags = flags;
        }

        /// <summary>
        /// Gets a value indicating whether the connection is available to all users.
        /// </summary>
        public virtual bool AllUsers => flags.HasFlag(RASCF.AllUsers);

        /// <summary>
        /// Gets a value indicating whether the credentials used for the connection are the default credentials.
        /// </summary>
        public virtual bool GlobalCredentials => flags.HasFlag(RASCF.GlobalCreds);

        /// <summary>
        /// Gets a value indicating whether the owner of the connection is known.
        /// </summary>
        public virtual bool OwnerKnown => flags.HasFlag(RASCF.OwnerKnown);

        /// <summary>
        /// Gets a value indicating whether the owner of the connection matches the current user.
        /// </summary>
        public virtual bool OwnerMatch => flags.HasFlag(RASCF.OwnerMatch);
    }
}