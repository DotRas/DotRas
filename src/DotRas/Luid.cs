using System;
using System.Globalization;
using System.Runtime.InteropServices;
using DotRas.Internal;
using DotRas.Internal.Abstractions.Services;

namespace DotRas
{
    /// <summary>
    /// Represents a locally unique identifier (LUID).
    /// </summary>
    /// <remarks>A <see cref="Luid"/> is guaranteed to be unique only on the system on which it was generated. Also, the uniqueness of a <see cref="Luid"/> is guaranteed only until the system is restarted.</remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Luid : IEquatable<Luid>, IFormattable
    {
        #region Fields and Properties

        /// <summary>
        /// Represents an empty <see cref="Luid"/>. This field is read-only.
        /// </summary>
        public static readonly Luid Empty = new Luid(0, 0);

        private readonly uint lowPart;
        private readonly int highPart;

        #endregion

        internal Luid(uint lowPart, int highPart)
        {
            this.lowPart = lowPart;
            this.highPart = highPart;
        }

        /// <summary>
        /// Evaluates the equality of two instances.
        /// </summary>
        /// <param name="objA">The first instance.</param>
        /// <param name="objB">The second instance.</param>
        /// <returns>A value indicating whether the two objects are equal.</returns>
        public static bool operator ==(Luid objA, Luid objB)
        {
            return objA.Equals(objB);
        }

        /// <summary>
        /// Evaluates the inequality of two instances.
        /// </summary>
        /// <param name="objA">The first instance.</param>
        /// <param name="objB">The second instance.</param>
        /// <returns>A value indicating whether the two objects are not equal.</returns>
        public static bool operator !=(Luid objA, Luid objB)
        {
            return !(objA == objB);
        }

        /// <summary>
        /// Generates a new locally unique identifier.
        /// </summary>
        /// <returns>A new <see cref="Luid"/> structure.</returns>
        public static Luid NewLuid()
        {
            return ServiceLocator.Default.GetRequiredService<IAllocateLocallyUniqueId>()
                .AllocateLocallyUniqueId();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare the current instance to.</param>
        /// <returns><b>true</b> if the objects are equal, otherwise <b>false</b>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Luid luid)
            {
                return Equals(luid);
            }

            return false;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other">The <see cref="Luid"/> to compare the current instance to.</param>
        /// <returns><b>true</b> if the objects are equal, otherwise <b>false</b>.</returns>
        public bool Equals(Luid other)
        {
            return ToInt64() == other.ToInt64();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToInt64().GetHashCode();
        }

        /// <summary>
        /// Converts the <see cref="Luid"/> to a 64-bit signed integer.
        /// </summary>
        /// <returns>The 64-bit signed integer.</returns>
        public long ToInt64()
        {
            return (long)highPart << 32 | lowPart;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToString("G", CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToInt64().ToString(format, formatProvider);
        }
    }
}