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
    /// An extension of <see cref="IDisposable" />.
    /// </summary>
    public interface IDisposableObject : IServerObject, IDisposable
    {
        #region Events (2)

        /// <summary>
        /// Is invoked AFTER object has been disposed.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Is invoked BEFORE object starts disposing.
        /// </summary>
        event EventHandler Disposing;

        #endregion Events (2)

        #region Properties (1)

        /// <summary>
        /// Gets if that object has been disposed or not.
        /// </summary>
        bool IsDisposed { get; }

        #endregion Properties (1)
    }
}