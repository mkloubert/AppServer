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
using System.ComponentModel;

namespace MarcelJoachimKloubert.AppServer.SDK
{
    /// <summary>
    /// The mother of all objects.
    /// </summary>
    public interface IServerObject : INotifyPropertyChanged
    {
        #region Events (1)

        /// <summary>
        /// Is invoked if one or more errors
        /// </summary>
        event EventHandler<ErrorsReceivedEventArgs> ErrorsReceived;

        #endregion Events (1)

        #region Properties (1)

        /// <summary>
        /// Gets if that object works thread safe or not.
        /// </summary>
        bool IsSynchronized { get; }

        /// <summary>
        /// Gets the object for thread safe / lock operations.
        /// </summary>
        object SyncRoot { get; }

        #endregion Properties (1)
    }
}