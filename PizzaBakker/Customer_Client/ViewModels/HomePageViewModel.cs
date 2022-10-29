using System.Collections.Generic;
using System.Windows.Input;
using Customer_Client.Commands;
using Customer_Client.Stores;
using Shared;

namespace Customer_Client.ViewModels;

public class HomePageViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    private string _naam;
    public string naam
    {
        get => _naam;
        set
        {
            _naam = value;
            OnPropertyChanged(nameof(naam));
        }
    }
    
    private List<Pizza> _allPizzas;
    public List<Pizza> AllPizzas
    {
        get => _allPizzas;
        set
        {
            _allPizzas = value;
            OnPropertyChanged(nameof(AllPizzas));
        }
    }

    private Pizza _selectedPizza;
    public Pizza SelectedPizza
    {
        get => _selectedPizza;
        set
        {
            _selectedPizza = value;
            OnPropertyChanged(nameof(SelectedPizza));
        }
    }
    
    public ICommand InitCommand { get; }
    public ICommand PlaceOrderCommand { get; }

    public HomePageViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        
        PlaceOrderCommand = new PlaceOrderCommand(_navigationStore);
        InitCommand = new InitCommand(_navigationStore);

        AllPizzas = new List<Pizza>();
        naam = "sadas";
        
        InitCommand.Execute(null);
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
    }
}