using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Division42.NetworkTools.Whois;

namespace Division42.NetworkTools.UIWinWpf.ViewModels
{
    public class WhoisViewModel : ObservableBase, ICommand
    {

        public IWhoisManager CurrentWhoisManager { get; set; }

        /// <summary>
        /// Gets or sets hostname.
        /// </summary>
        public String DomainName
        {
            get { return _domainName; }
            set
            {
                _domainName = value;
                OnPropertyChanged("DomainName");
                OnCanExecuteChanged();
            }
        } private String _domainName = default(String);

        /// <summary>
        /// Gets or sets the result of the whois call.
        /// </summary>
        public String WhoisResult
        {
            get { return _whoisResult; }
            protected set
            {
                _whoisResult = value;
                OnPropertyChanged("WhoisResult");
                OnCanExecuteChanged();
            }
        } private String _whoisResult = default(String);

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
            if (!String.IsNullOrEmpty(DomainName) && _canExecute)
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
            OnCanExecuteChanged();

            CurrentWhoisManager = new WhoisManager();

            Task.Factory.StartNew(() =>
            {
                String result = CurrentWhoisManager.ExecuteWhoisForDomain("=" + DomainName);

                IEnumerable<String> whoisServers = CurrentWhoisManager.FindWhoisServerInOutput(result);

                if (whoisServers != null && whoisServers.Count() > 0)
                {
                    String actualResult = CurrentWhoisManager.ExecuteWhoisForDomain(DomainName, whoisServers.First());

                    result = actualResult;
                }

                App.CurrentDispatcher.Invoke(() =>
                {
                    this.WhoisResult = result;

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