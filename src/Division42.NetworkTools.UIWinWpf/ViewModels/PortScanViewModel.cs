using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Division42.NetworkTools.PortScan;

namespace Division42.NetworkTools.UIWinWpf.ViewModels
{
    public class PortScanViewModel : ObservableBase, ICommand
    {
        public PortScanViewModel()
        {
            PortScanResults = new ObservableCollection<PortScanResultEventArgs>();
        }

        public IPortScanManager CurrentPortScanManager { get; set; }

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

        public ObservableCollection<PortScanResultEventArgs> PortScanResults { get; protected set; }

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
            OnCanExecuteChanged();

            CurrentPortScanManager = new PortScanManager(HostName);
            PortScanResults.Clear();
            CurrentPortScanManager.PortScanResult += (sender, e) =>
            {
                App.CurrentDispatcher.Invoke(() =>
                {
                    PortScanResults.Add(e);
                });
            };

            CurrentPortScanManager.Start(1,600, PortTypes.Tcp);

            Task.Factory.StartNew(() =>
            {
                Task.WaitAll(CurrentPortScanManager.Tasks.ToArray());

                App.CurrentDispatcher.Invoke(() =>
                {
                    _canExecute = true;
                    OnCanExecuteChanged();
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