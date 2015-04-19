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
    /// Describes an object that can be (re)started and stopped.
    /// </summary>
    public interface IRunnable : IServerObject
    {
        #region Events (2)

        /// <summary>
        /// Is invoked AFTER object has been started.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// Is invoked BEFORE object starts working.
        /// </summary>
        event EventHandler Starting;

        #endregion Events (2)

        #region Properties (2)

        /// <summary>
        /// Gets if the object can be started or not.
        /// </summary>
        bool CanStart { get; }

        /// <summary>
        /// Gets if the object is running or not.
        /// </summary>
        bool IsRunning { get; }

        #endregion Properties (2)

        #region Methods (2)

        /// <summary>
        /// Starts the object.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Object cannot be started.
        /// </exception>
        void Start();

        #endregion Methods (2)
    }
}