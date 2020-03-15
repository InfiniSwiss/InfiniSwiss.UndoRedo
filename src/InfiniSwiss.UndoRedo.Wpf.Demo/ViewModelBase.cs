using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace InfiniSwiss.UndoRedo.Wpf.Demo
{
    /// <summary>
    /// Represets the base of ViewModels with an implementation for <seealso cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object?> fields = new Dictionary<string, object?>();

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Sets the value associated with the specified propertyName.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="value">The value to be set.</param>
        /// <param name="propertyName">The property name.</param>
        protected void Set<TValue>([AllowNull]TValue value, [CallerMemberName] string? propertyName = null)
        {
            this.fields.TryGetValue(propertyName!, out var oldValue);
            if (oldValue?.Equals(value) == true)
            {
                return;
            }

            this.fields[propertyName!] = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName!));
        }

        /// <summary>
        /// Gets a value for a property with a nullable reference type.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The value of the property.</returns>
        protected TValue? GetValue<TValue>([CallerMemberName] string? propertyName = null)
            where TValue : class
        {
            if (this.fields.TryGetValue(propertyName!, out var value) && value is TValue tvalue)
            {
                return tvalue;
            }

            return default;
        }

        /// <summary>
        /// Gets a value for a property with a non-nullable reference type or a value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="defaultValue">The default value for the non-nullable reference type.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The value of the property.</returns>
        protected TValue GetValue<TValue>(TValue defaultValue, [CallerMemberName] string? propertyName = null)
        {
            if (this.fields.TryGetValue(propertyName!, out var value) && value is TValue tvalue)
            {
                return tvalue;
            }

            return defaultValue;
        }
    }
}
