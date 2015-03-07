using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Division42.NetworkTools.UIWinWpf.Pages.Settings;

namespace Division42.NetworkTools.UIWinWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


/// <summary>
/// Gets the current AppearanceViewModel.
/// </summary>
public static AppearanceViewModel CurrentAppearanceViewModel
{
    get
    {
        if (_currentAppearanceViewModel == null)
            _currentAppearanceViewModel = new AppearanceViewModel();

        return _currentAppearanceViewModel;
    }
} private static AppearanceViewModel _currentAppearanceViewModel = null;

        /// <summary>
        /// Gets the current Dispatcher.
        /// </summary>
        public static Dispatcher CurrentDispatcher
        {
            get
            {
                return _currentDispatcher;
            }
        } private static Dispatcher _currentDispatcher = Dispatcher.CurrentDispatcher;
    }
}