using Customer_Client.Commands;
using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.UI_Element;
using Shared;
using System.Collections.Generic;
using System.Windows.Input;

namespace Customer_Client.ViewModels;

public class HomePageViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    private string _naam;
    public string Naam
    {
        get => _naam;
        set
        {
            _naam = value;
            OnPropertyChanged(nameof(Naam));
        }
    }

    private List<Pizza> _orderedPizzas;
    public List<Pizza> OrderedPizzas { get => _orderedPizzas; }

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

    private List<PizzaListItem> _pizzaList;
    public List<PizzaListItem> PizzaListItems
    {
        get => _pizzaList; set
        {
            _pizzaList = value;
            OnPropertyChanged(nameof(PizzaListItems));
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

    public HomePageViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        InitCommand = new InitCommand(_navigationStore);
        AllPizzas = new List<Pizza>();
        Naam = UserInfo.Instance.UserName;
        InitCommand.Execute(null);

    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
    }
}