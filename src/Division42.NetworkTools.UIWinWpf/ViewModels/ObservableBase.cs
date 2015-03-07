using System;
using System.ComponentModel;

namespace Division42.NetworkTools.UIWinWpf.ViewModels
{
    /// <summary>
    /// Abstract base class for observeable models and viewmodels.
    /// </summary>
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the <see cref="PropertyChanged"/> event for the 
        /// specified <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property that 
        /// just changed.</param>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void OnPropertyChanged(String propertyName)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("Argument \"propertyName\" cannot be null or empty.", "propertyName");

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}