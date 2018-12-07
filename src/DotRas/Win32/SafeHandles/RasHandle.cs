using System;
using Microsoft.Win32.SafeHandles;

namespace DotRas.Win32.SafeHandles
{
    /// <summary>
    /// Represents a remote access service connection handle.
    /// </summary>
    public sealed class RasHandle : SafeHandleZeroOrMinusOneIsInvalid, IEquatable<RasHandle>
    {
        public RasHandle()
            : base(false)
        {
        }

        public static RasHandle FromPtr(IntPtr lprasconn)
        {
            if (lprasconn == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(lprasconn));
            }

            RasHandle result = null;

            try
            {
                result = new RasHandle();
                result.SetHandle(lprasconn);

                return result;
            }
            catch (Exception)
            {
                result?.Dispose();
                throw;
            }
        }

        protected override bool ReleaseHandle()
        {
            return false;
        }

        public override string ToString()
        {
            return handle.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is RasHandle other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(RasHandle other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.handle == handle;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            // Reason: Unable to make the field readonly as it is owned by the base class.
            return handle.GetHashCode();
        }

        public static bool operator ==(RasHandle objA, RasHandle objB)
        {
            if (ReferenceEquals(objA, objB))
            {
                return true;
            }

            if (objA is null || objB is null)
            {
                return false;
            }

            return objA.Equals(objB);
        }

        public static bool operator !=(RasHandle objA, RasHandle objB)
        {
            return !(objA == objB);
        }
    }
}