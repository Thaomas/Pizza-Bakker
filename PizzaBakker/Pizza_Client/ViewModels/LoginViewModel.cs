using Employee_Client.Commands;
using Employee_Client.Stores;
using Shared;
using System.Windows.Input;

namespace Employee_Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            LoginCommand = new LoginCommand(_navigationStore);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
        }
    }
}
