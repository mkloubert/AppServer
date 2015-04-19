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
    /// A basic application server.
    /// </summary>
    public abstract class ApplicationServerBase : DisposableObjectBase, IApplicationServer
    {
        #region Constructors (2)

        /// <inheriteddoc />
        protected ApplicationServerBase()
            : base()
        {
        }

        /// <inheriteddoc />
        protected ApplicationServerBase(object sync)
            : base(sync: sync)
        {
        }

        #endregion Constructors (2)

        #region Events (2)

        /// <inheriteddoc />
        public event EventHandler Started;

        /// <inheriteddoc />
        public event EventHandler Starting;

        #endregion Events (2)

        #region Properties (2)

        /// <inheriteddoc />
        public virtual bool CanStart
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public bool IsRunning
        {
            get { return this.Get(() => this.IsRunning); }

            private set { this.Set(value, () => this.IsRunning); }
        }

        #endregion Properties (2)

        #region Methods (2)

        /// <summary>
        /// The logic for the <see cref="ApplicationServerBase.Start()" /> method.
        /// </summary>
        /// <param name="isRunning">
        /// The value for the <see cref="ApplicationServerBase.IsRunning" /> property.
        /// </param>
        protected virtual void OnStart(ref bool isRunning)
        {
        }

        /// <inheriteddoc />
        public void Start()
        {
            lock (this._SYNC)
            {
                if (this.IsRunning)
                {
                    return;
                }

                this.ThrowIfDisposed();

                if (this.CanStart == false)
                {
                    throw new InvalidOperationException("Server cannot be started!");
                }

                this.RaiseEventHandler(this.Starting);

                var isRunning = true;
                this.OnStart(ref isRunning);

                this.IsRunning = isRunning;
                if (isRunning)
                {
                    this.RaiseEventHandler(this.Started);
                }
            }
        }

        #endregion Methods (2)
    }
}