using Customer_Client.Stores;
using Shared;

namespace Customer_Client.ViewModels;

public class HomePageViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;

    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    public HomePageViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
    }
}