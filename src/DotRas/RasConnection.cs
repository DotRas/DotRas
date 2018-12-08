using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.DependencyInjection;
using DotRas.Win32.SafeHandles;

namespace DotRas
{
    public class RasConnection
    {
        internal RasConnection(RasHandle handle, Device device, string entryName, string phoneBook)
        {
            if (handle == null)
            {
                throw new ArgumentNullException(nameof(handle));
            }
            else if (handle.IsClosed)
            {
                throw new ArgumentException("The handle provided must not be closed.", nameof(handle));
            }
            else if (handle.IsInvalid)
            {
                throw new ArgumentException("The handle is invalid.", nameof(handle));
            }
            else if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }
            else if (string.IsNullOrWhiteSpace(phoneBook))
            {
                throw new ArgumentNullException(nameof(phoneBook));
            }

            EntryName = entryName;
            PhoneBook = phoneBook;
            Handle = handle;
            Device = device ?? throw new ArgumentNullException(nameof(device));
        }
        
        protected RasConnection()
        {
        }

        public virtual RasHandle Handle { get; }
        public virtual Device Device { get; }
        public virtual string EntryName { get; }
        public virtual string PhoneBook { get; }

        public static IEnumerable<RasConnection> EnumerateConnections()
        {
            return Container.Default.GetRequiredService<IRasEnumConnections>()
                .EnumerateConnections();
        }

        public virtual ConnectionStatus GetStatus()
        {
            GuardHandleMustBeValid();

            return Container.Default.GetRequiredService<IRasGetConnectStatus>()
                .GetConnectionStatus(Handle);
        }

        public virtual Task DisconnectAsync(CancellationToken cancellationToken)
        {
            GuardHandleMustBeValid();

            return Container.Default.GetRequiredService<IRasHangUp>()
                .HangUpAsync(Handle, cancellationToken);
        }

        private void GuardHandleMustBeValid()
        {
            if (Handle.IsClosed || Handle.IsInvalid)
            {
                throw new Exception("The handle is invalid.");
            }
        }
    }
}