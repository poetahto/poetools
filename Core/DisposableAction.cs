using System;

namespace poetools.Core
{
    /// <summary>
    /// A disposable that runs a callback when disposed.
    /// </summary>
    public class DisposableAction : IDisposable
    {
        /// <summary>
        /// The action that should be invoked when this object is disposed.
        /// </summary>
        public Action Action;

        /// <summary>
        /// Invokes the associated <see cref="Action"/>
        /// </summary>
        public virtual void Dispose()
        {
            Action?.Invoke();
        }
    }

    /// <summary>
    /// A disposable that only runs a callback the first time it is disposed.
    /// </summary>
    public class DisposableActionOneShot : DisposableAction
    {
        /// <summary>
        /// Gets a value indicating whether this object has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                base.Dispose();
            }
        }

        /// <summary>
        /// Un-sets the <see cref="IsDisposed"/> flag, so that the action can be triggered again.
        /// </summary>
        public void Reset()
        {
            IsDisposed = false;
        }
    }
}
