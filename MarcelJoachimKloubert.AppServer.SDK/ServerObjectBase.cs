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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace MarcelJoachimKloubert.AppServer.SDK
{
    /// <summary>
    /// A basic object.
    /// </summary>
    public abstract class ServerObjectBase : MarshalByRefObject, IServerObject
    {
        #region Fields (2)

        private readonly IDictionary<string, object> _PROPERTIES;

        /// <summary>
        /// Stores the object for <see cref="ServerObjectBase.SyncRoot" /> property.
        /// </summary>
        protected readonly object _SYNC;

        #endregion Fields (2)

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of <see cref="ServerObjectBase" /> class.
        /// </summary>
        protected ServerObjectBase()
            : this(sync: new object())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerObjectBase" /> class.
        /// </summary>
        /// <param name="sync">the value for <see cref="ServerObjectBase._SYNC" /> field.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sync" /> is <see langword="null" />.
        /// </exception>
        protected ServerObjectBase(object sync)
        {
            if (sync == null)
            {
                throw new ArgumentNullException("sync");
            }

            this._PROPERTIES = this.CreatePropertyStorage();
            this._SYNC = sync;
        }

        #endregion Constructors (2)

        #region Events and delegates (3)

        /// <summary>
        /// Describes an object that provides the default value for a property.
        /// </summary>
        /// <typeparam name="T">Type of the default value.</typeparam>
        /// <param name="obj">The underlying object.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The default value.</returns>
        protected delegate T DefaultPropertyValueProvider<T>(ServerObjectBase obj, string propertyName);

        /// <inheriteddoc />
        public event EventHandler<ErrorsReceivedEventArgs> ErrorsReceived;

        /// <inheriteddoc />
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events and delegates (3)

        #region Properties (2)

        /// <inheriteddoc />
        public virtual bool IsSynchronized
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public object SyncRoot
        {
            get { return this._SYNC; }
        }

        #endregion Properties (2)

        #region Methods (17)

        private void CheckPropertyOrThrow(string propertyName)
        {
            if (global::System.ComponentModel.TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new global::System.MissingMemberException(className: this.GetType().FullName,
                                                                memberName: propertyName);
            }
        }

        /// <summary>
        /// Creates the instance of the internal property storage.
        /// </summary>
        /// <returns>The created instance.</returns>
        protected virtual IDictionary<string, object> CreatePropertyStorage()
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns the value of a property.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="expr">The name of the property as expression.</param>
        /// <param name="defaultValue">
        /// The default value if no value is currently set.
        /// </param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> contains no <see cref="MemberExpression" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expr" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// <paramref name="expr" /> is NO property expression.
        /// </exception>
        protected T Get<T>(Expression<Func<T>> expr, T defaultValue = default(T))
        {
            return this.Get<T>(propertyName: GetPropertyName<T>(expr),
                               defaultValue: defaultValue);
        }

        /// <summary>
        /// Returns the value of a property.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="expr">The name of the property as expression.</param>
        /// <param name="defaultValueProvider">
        /// The function that provides the default value if no value is currently set.
        /// </param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> contains no <see cref="MemberExpression" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expr" /> and/or <paramref name="defaultValueProvider" /> are <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// <paramref name="expr" /> is NO property expression.
        /// </exception>
        protected T Get<T>(Expression<Func<T>> expr, DefaultPropertyValueProvider<T> defaultValueProvider)
        {
            return this.Get<T>(propertyName: GetPropertyName<T>(expr),
                               defaultValueProvider: defaultValueProvider);
        }

        /// <summary>
        /// Returns the value of a property.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="defaultValue">
        /// The default value if no value is currently set.
        /// </param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyName" /> is empty / invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName" /> is <see langword="null" />.
        /// </exception>
        protected T Get<T>(string propertyName, T defaultValue = default(T))
        {
            return this.Get<T>(propertyName: propertyName,
                               defaultValueProvider: (obj, properties) => defaultValue);
        }

        /// <summary>
        /// Returns the value of a property.
        /// </summary>
        /// <typeparam name="T">Result type.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="defaultValueProvider">
        /// The function that provides the default value if no value is currently set.
        /// </param>
        /// <returns>The value of the property.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyName" /> is empty / invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName" /> and/or <paramref name="defaultValueProvider" /> are <see langword="null" />.
        /// </exception>
        protected T Get<T>(string propertyName, DefaultPropertyValueProvider<T> defaultValueProvider)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            if (defaultValueProvider == null)
            {
                throw new ArgumentNullException("defaultValueProvider");
            }

            propertyName = propertyName.Trim();
            if (propertyName == string.Empty)
            {
                throw new ArgumentException("propertyName");
            }

#if DEBUG

            this.CheckPropertyOrThrow(propertyName);

#endif

            return this.InvokeThreadSafe((obj, state) =>
                {
                    object value;
                    if (obj._PROPERTIES.TryGetValue(state.PropertyName, out value))
                    {
                        return (T)value;
                    }

                    return state.DefaultValueProvider(obj, state.PropertyName);
                }, new
                {
                    DefaultValueProvider = defaultValueProvider,
                    PropertyName = propertyName,
                });
        }

        /// <summary>
        /// Returns the name of a property by using an expression.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="expr">The expression that provides the property name.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="expr" /> contains no <see cref="MemberExpression" />.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expr" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// <paramref name="expr" /> is NO property expression.
        /// </exception>
        protected static string GetPropertyName<T>(Expression<Func<T>> expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException("expr");
            }

            var memberExpr = expr.Body as MemberExpression;
            if (memberExpr == null)
            {
                throw new ArgumentException("expr.Body");
            }

            return ((PropertyInfo)memberExpr.Member).Name;
        }

        /// <summary>
        /// Invokes an action thread safe.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        protected void InvokeThreadSafe(Action<ServerObjectBase> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.InvokeThreadSafe(action: (obj, state) => state.Action(obj),
                                  actionState: new
                                  {
                                      Action = action,
                                  });
        }

        /// <summary>
        /// Invokes an action thread safe.
        /// </summary>
        /// <typeparam name="T">Type of the second argument for <paramref name="action" />.</typeparam>
        /// <param name="action">The action to invoke.</param>
        /// <param name="actionState">
        /// The value for the 2nd argument of <paramref name="action" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> is <see langword="null" />.
        /// </exception>
        protected void InvokeThreadSafe<T>(Action<ServerObjectBase, T> action, T actionState)
        {
            this.InvokeThreadSafe(action: action,
                                  actionStateProvider: (obj) => actionState);
        }

        /// <summary>
        /// Invokes an action thread safe.
        /// </summary>
        /// <typeparam name="T">Type of the second argument for <paramref name="action" />.</typeparam>
        /// <param name="action">The action to invoke.</param>
        /// <param name="actionStateProvider">
        /// The function that provides the value for the 2nd argument of <paramref name="action" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action" /> and/or <paramref name="actionStateProvider" /> are <see langword="null" />.
        /// </exception>
        protected void InvokeThreadSafe<T>(Action<ServerObjectBase, T> action, Func<ServerObjectBase, T> actionStateProvider)
        {
            this.InvokeThreadSafe(
                func: (obj, state) =>
                {
                    state.Action(obj,
                                 state.StateProvider(obj));

                    return (object)null;
                },
                funcState: new
                {
                    Action = action,
                    StateProvider = actionStateProvider,
                });
        }

        /// <summary>
        /// Invokes a function thread safe.
        /// </summary>
        /// <typeparam name="R">The type of the result of <paramref name="func" />.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <returns>The result of <paramref name="func" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        protected R InvokeThreadSafe<R>(Func<ServerObjectBase, R> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            return this.InvokeThreadSafe(func: (obj, state) => state.Function(obj),
                                         funcState: new
                                         {
                                             Function = func,
                                         });
        }

        /// <summary>
        /// Invokes a function thread safe.
        /// </summary>
        /// <typeparam name="T">Type of the second argument for <paramref name="func" />.</typeparam>
        /// <typeparam name="R">The type of the result of <paramref name="func" />.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="funcState">
        /// The value for the 2nd argument of <paramref name="func" />.
        /// </param>
        /// <returns>The result of <paramref name="func" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> is <see langword="null" />.
        /// </exception>
        protected R InvokeThreadSafe<T, R>(Func<ServerObjectBase, T, R> func, T funcState)
        {
            return this.InvokeThreadSafe(func: func,
                                         funcStateProvider: (obj) => funcState);
        }

        /// <summary>
        /// Invokes a function thread safe.
        /// </summary>
        /// <typeparam name="T">Type of the second argument for <paramref name="func" />.</typeparam>
        /// <typeparam name="R">The type of the result of <paramref name="func" />.</typeparam>
        /// <param name="func">The function to invoke.</param>
        /// <param name="funcStateProvider">
        /// The function that provides the value for the 2nd argument of <paramref name="func" />.
        /// </param>
        /// <returns>The result of <paramref name="func" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func" /> and/or <paramref name="funcStateProvider" /> are <see langword="null" />.
        /// </exception>
        protected R InvokeThreadSafe<T, R>(Func<ServerObjectBase, T, R> func, Func<ServerObjectBase, T> funcStateProvider)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            if (funcStateProvider == null)
            {
                throw new ArgumentNullException("funcStateProvider");
            }

            R result;

            lock (this._SYNC)
            {
                result = func(this,
                              funcStateProvider(this));
            }

            return result;
        }

        /// <summary>
        /// Raises the <see cref="ServerObjectBase.ErrorsReceived" /> event.
        /// </summary>
        /// <param name="ex">The occured error(s).</param>
        /// <returns>Event was raised or not.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex" /> is <see langword="null" />.
        /// </exception>
        protected bool OnErrorsReceived(Exception ex)
        {
            // (null) check is done by constructor
            // of ErrorsReceivedEventArgs class

            var handler = this.ErrorsReceived;
            if (handler != null)
            {
                handler(this, new ErrorsReceivedEventArgs(ex));
            }

            return false;
        }

        /// <summary>
        /// Raises the <see cref="ServerObjectBase.PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>Event was raised or not.</returns>
        protected bool OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Raises an event handler.
        /// </summary>
        /// <param name="handler">The handler to raise.</param>
        /// <returns>Handler was raised or not.</returns>
        protected bool RaiseEventHandler(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="newValue">The (new) value for the property.</param>
        /// <param name="expr">The name of the property as expression.</param>
        /// <returns>
        /// New value is different to old one or not (<see cref="ServerObjectBase.PropertyChanged" /> event was raised).
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="expr" /> is <see langword="null" />.
        /// </exception>
        protected bool Set<T>(T newValue, Expression<Func<T>> expr)
        {
            return this.Set<T>(newValue: newValue,
                               propertyName: GetPropertyName<T>(expr));
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">Type of the property value.</typeparam>
        /// <param name="newValue">The (new) value for the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// New value is different to old one or not (<see cref="ServerObjectBase.PropertyChanged" /> event was raised).
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyName" /> is empty / invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName" /> is <see langword="null" />.
        /// </exception>
        protected bool Set<T>(T newValue, string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            propertyName = propertyName.Trim();
            if (propertyName == string.Empty)
            {
                throw new ArgumentException("propertyName");
            }

#if DEBUG

            this.CheckPropertyOrThrow(propertyName);

#endif

            return this.InvokeThreadSafe((obj, state) =>
                {
                    object value;
                    obj._PROPERTIES
                       .TryGetValue(state.PropertyName, out value);

                    var result = object.Equals(value, state.NewValue) == false;

                    if (result)
                    {
                        obj._PROPERTIES[state.PropertyName] = state.NewValue;

                        obj.OnPropertyChanged(state.PropertyName);
                    }

                    return result;
                }, new
                {
                    NewValue = newValue,
                    PropertyName = propertyName,
                });
        }

        #endregion Methods (17)
    }
}