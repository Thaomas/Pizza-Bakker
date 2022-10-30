using Customer_Client.Commands;
using Customer_Client.Logic;
using Customer_Client.Stores;
using Shared;
using System.Threading;
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

        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set
            {
                _buttonEnabled = value;
                OnPropertyChanged(nameof(ButtonEnabled));
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            LoginCommand = new LoginCommand(_navigationStore);
            ButtonEnabled = true;
            Label = "Login";

            if (UserInfo.LoadUserInfo())
            {
                ChangeViewModel();
            }
        }

        public void ChangeViewModel()
        {
            new Thread(() =>
            {
                ButtonEnabled = false;
                Name = UserInfo.Instance.UserName;
                while (!ConnectionHandler.GetInstance().IsConnected) ;
                BaseViewModel viewModel = new HomePageViewModel(_navigationStore);
                _navigationStore.CurrentViewModel = viewModel;
            }).Start();

        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
        }
    }
}
