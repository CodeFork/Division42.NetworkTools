using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Division42.NetworkTools.TraceRoute;

namespace Division42.NetworkTools.UIWinWpf.ViewModels
{
    public class TraceRouteViewModel : ObservableBase, ICommand
    {
        public TraceRouteViewModel()
        {
            TraceRouteResults = new ObservableCollection<TraceRouteHopDetail>();
        }

        public ITraceRouteManager CurrentTraceRouteManager { get; set; }

        /// <summary>
        /// Gets or sets hostname.
        /// </summary>
        public String HostName
        {
            get { return _hostName; }
            set
            {
                _hostName = value;
                OnPropertyChanged("HostName");
                OnCanExecuteChanged();
            }
        } private String _hostName = default(String);

        /// <summary>
        /// Gets whether this instance is currently executing.
        /// </summary>
        public Boolean IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        } private Boolean _isActive = false;

        public ObservableCollection<TraceRouteHopDetail> TraceRouteResults { get; protected set; }

        /// <summary>
        /// Defines the method that determines whether the command can 
        /// execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the 
        /// command does not require data to be passed, this object 
        /// can be set to null.</param>
        public Boolean CanExecute(object parameter)
        {
            if (!String.IsNullOrEmpty(HostName) && _canExecute)
                return true;
            else
                return false;
        } Boolean _canExecute = true;

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the 
        /// command does not require data to be passed, this object 
        /// can be set to null.</param>
        public async void Execute(object parameter)
        {
            _canExecute = false;
            IsActive = true;
            OnCanExecuteChanged();

            CurrentTraceRouteManager = new TraceRouteManager();
            
            TraceRouteResults.Clear();
            CurrentTraceRouteManager.TraceRouteNodeFound += (sender, e) =>
            {
                App.CurrentDispatcher.Invoke(() =>
                {
                    TraceRouteResults.Add(e.Detail);
                });
            };
            await CurrentTraceRouteManager.ExecuteTraceRoute(HostName);
            
            _canExecute = true;
            OnCanExecuteChanged();

            IsActive = false;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the 
        /// command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;


        protected void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }
    }
}