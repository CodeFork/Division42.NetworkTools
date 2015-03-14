using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using Division42.NetworkTools.IcmpPing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.UIWinWpf.ViewModels
{
    public class PingResultViewModel : ObservableBase, ICommand
    {
        public PingResultViewModel()
        {
            PingResults = new ObservableCollection<PingResultEventArgs>();
            IsActive = false;
        }

        public IPingManager CurrentPingManager { get; set; }

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

        public ObservableCollection<PingResultEventArgs> PingResults { get; protected set; }

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
        public void Execute(object parameter)
        {
            _canExecute = false;
            IsActive = true;
            OnCanExecuteChanged();

            CurrentPingManager = new PingManager(HostName, TimeSpan.FromSeconds(1));
            PingResults.Clear();
            CurrentPingManager.PingResult += (sender, e) =>
            {
                App.CurrentDispatcher.Invoke(() =>
                {
                    PingResults.Add(e);
                });
            };

            CurrentPingManager.Start();
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                CurrentPingManager.Stop();


                App.CurrentDispatcher.Invoke(() =>
                {
                    _canExecute = true;
                    OnCanExecuteChanged();
                    IsActive = false;
                });
            });
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
