using System;
using System.ComponentModel;

namespace DotRas
{
    /// <summary>
    /// Provides a base class for remote access service (RAS) component classes. This class must be inherited.
    /// </summary>
    public abstract class RasComponentBase : DisposableObject
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the object used to marshal events that are raised by the component.
        /// </summary>
        public ISynchronizeInvoke SynchronizingObject { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when an exception occurs while an event is being handled.
        /// </summary>
        /// <remarks>Please note, this event ensures no errors will destabilize the RAS subsystem within the operating system.</remarks>
        public event EventHandler<ErrorEventArgs> Error;

        #endregion

        /// <summary>
        /// Raises the <see cref="Error"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ErrorEventArgs"/> containing event data.</param>
        protected void RaiseErrorEvent(ErrorEventArgs e)
        {
            try
            {
                RaiseEvent(Error, e);
            }
            catch (Exception)
            {
                // Swallow any exceptions which occur while handling the error event to prevent the exception bubbling back up to the RAS subsystem.
            }
        }

        /// <summary>
        /// Raises the event specified by <paramref name="method"/> with the event data provided. 
        /// </summary>
        /// <typeparam name="TEventArgs">The <see cref="EventArgs"/> used by the event delegate.</typeparam>
        /// <param name="method">The event delegate being raised.</param>
        /// <param name="e">An <typeparamref name="TEventArgs"/> containing event data.</param>
        protected void RaiseEvent<TEventArgs>(EventHandler<TEventArgs> method, TEventArgs e)
            where TEventArgs : EventArgs
        {
            if (method == null)
            {
                // The event may not have been attached to by the developer, do not worry about it.
                return;
            }

            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
            {
                SynchronizingObject.Invoke(method, new object[] { this, e });
            }
            else
            {
                method(this, e);
            }
        }
    }
}