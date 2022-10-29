using Customer_Client.Commands;
using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.Util;
using Shared;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Customer_Client.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
        private UserInfo info;

        private string _userName = "";
        public string Name
        {
            get => _userName; set
            {
                _userName = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _label;
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged(Label);
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            LoginCommand = new LoginCommand(_navigationStore);
            Label = "Login";

            if (UserInfo.LoadUserInfo())
            {
                ChangeViewModel();
            }
        }

        public void ChangeViewModel()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                while (!ConnectionHandler.GetInstance().IsConnected) ;
                await Task.Delay(500);
                BaseViewModel viewModel = new HomePageViewModel(_navigationStore);
                _navigationStore.CurrentViewModel = viewModel;
            });
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
        }
    }
}
