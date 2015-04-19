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
    /// The arguments for an <see cref="IServerObject.ErrorsReceived" /> event.
    /// </summary>
    public sealed class ErrorsReceivedEventArgs : EventArgs
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorsReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="ex">The occured error(s).</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex" /> is <see langword="null" />.
        /// </exception>
        public ErrorsReceivedEventArgs(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            var aggEx = ex as AggregateException;
            if (aggEx == null)
            {
                aggEx = new AggregateException(ex);
            }

            this.Errors = aggEx;
        }

        #endregion Constructors (1)

        #region Properties (1)

        /// <summary>
        /// Gets the occured errors.
        /// </summary>
        public AggregateException Errors
        {
            get;
            private set;
        }

        #endregion Properties (1)
    }
}