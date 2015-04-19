// AppServer (https://github.com/mkloubert/AppServer)
// Copyright (c) Marcel Joachim Kloubert, All rights reserved.
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3.0 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library.

using System;

namespace MarcelJoachimKloubert.AppServer.SDK
{
    /// <summary>
    /// A basic disposable object.
    /// </summary>
    public abstract class DisposableObjectBase : ServerObjectBase, IDisposableObject
    {
        #region Constructors (3)

        /// <inheriteddoc />
        protected DisposableObjectBase()
            : base()
        {
        }

        /// <inheriteddoc />
        protected DisposableObjectBase(object sync)
            : base(sync: sync)
        {
        }

        /// <summary>
        /// The destructor.
        /// </summary>
        ~DisposableObjectBase()
        {
            this.Dispose(false);
        }

        #endregion Constructors (3)

        #region Events and delegates (2)

        /// <inheriteddoc />
        public event EventHandler Disposed;

        /// <inheriteddoc />
        public event EventHandler Disposing;

        #endregion Events and delegates (2)

        #region Properties (1)

        /// <inheriteddoc />
        public bool IsDisposed
        {
            get { return this.Get(() => this.IsDisposed); }

            private set { this.Set(value, () => this.IsDisposed); }
        }

        #endregion Properties (1)

        #region Methods (4)

        /// <inheriteddoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (this._SYNC)
            {
                if (disposing && this.IsDisposed)
                {
                    return;
                }

                if (disposing)
                {
                    this.RaiseEventHandler(this.Disposing);
                }

                bool isDisposed;
                if (disposing)
                {
                    isDisposed = true;
                }
                else
                {
                    isDisposed = this.IsDisposed;
                }

                this.OnDispose(disposing, ref isDisposed);

                if (disposing)
                {
                    this.IsDisposed = isDisposed;

                    if (isDisposed)
                    {
                        this.RaiseEventHandler(this.Disposed);
                    }
                }
            }
        }

        /// <summary>
        /// Stores the logic for the <see cref="DisposableObjectBase.Dispose()" /> method
        /// and the destructor.
        /// </summary>
        /// <param name="disposing">
        /// <see cref="DisposableObjectBase.Dispose()" /> method was called (<see langword="true" />)
        /// or the destructor  (<see langword="false" />).
        /// </param>
        /// <param name="isDisposed">
        /// The (new) value for <see cref="DisposableObjectBase.IsDisposed" /> property.
        /// </param>
        protected virtual void OnDispose(bool disposing, ref bool isDisposed)
        {
            // dummy
        }

        /// <summary>
        /// Throws an exception if that object has already been disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Object has been disposed.
        /// </exception>
        protected void ThrowIfDisposed()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(objectName: this.GetType().FullName,
                                                  message: string.Format("Instance {0} has already been disposed!",
                                                                         this.GetHashCode()));
            }
        }

        #endregion Methods (4)
    }
}