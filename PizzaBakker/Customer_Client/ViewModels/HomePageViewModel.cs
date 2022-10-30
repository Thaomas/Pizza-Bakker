using Customer_Client.Commands;
using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.UI_Element;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Customer_Client.ViewModels;

public class HomePageViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    public string Naam
    {
        get => _naam;
        set
        {
            _naam = value;
            OnPropertyChanged(nameof(Naam));
        }
    }
    private string _naam;

    private bool _selectedList = true;
    public int BasketRowSpan
    {
        get => (_selectedList) ? 1 : 2;
    }
    public Visibility BuyButtonVisibility
    {
        get => (_selectedList) ? Visibility.Visible : Visibility.Hidden;
    }

    public List<string> RightListViewList { get => _selectedList ? PizzasInBasket : OrderHistoryKeys; }

    public List<string> PizzasInBasket
    {
        get => _pizzasInBasket;
        set
        {
            _pizzasInBasket = value;
            OnPropertyChanged(nameof(RightListViewList));
        }
    }
    private List<string> _pizzasInBasket;

    public Dictionary<string, PizzaOrder> OrderHistory
    {
        get => _orderHistory;
        set
        {
            _orderHistory = value;
            OnPropertyChanged(nameof(RightListViewList));
        }
    }
    private Dictionary<string, PizzaOrder> _orderHistory = new Dictionary<string, PizzaOrder>();
    public List<string> OrderHistoryKeys
    {
        get => _orderHistory.Keys.ToList();
    }

    public string SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            _selectedOrder = value;
            OnPropertyChanged(nameof(SelectedOrder));
            if (_selectedOrder == null)
                return;
            _orderList.Clear();
            OrderHistory[_selectedOrder].AllPizzas.ForEach(p =>
            {
                List<string> ingredients;
                if (AllPizzas.TryGetValue(p, out ingredients))
                    _orderList.Add(new PizzaListItem(p, ingredients, _navigationStore, false));
                else
                    _orderList.Add(new PizzaListItem(p, new List<string>() { "Error, I dont exist" }, _navigationStore, false));
            });

            OnPropertyChanged(nameof(LeftListViewList));
        }
    }
    private string _selectedOrder;

    public Dictionary<string, List<string>> AllPizzas { get; set; }

    public List<PizzaListItem> LeftListViewList { get => _selectedList ? PizzaListItems : OrderListItems; }

    public List<PizzaListItem> PizzaListItems
    {
        get => _pizzaList; set
        {
            _pizzaList = value;
            OnPropertyChanged(nameof(PizzaListItems));
        }
    }
    private List<PizzaListItem> _pizzaList = new List<PizzaListItem>();

    public List<PizzaListItem> OrderListItems
    {
        get => _pizzaList; set
        {
            _pizzaList = value;
            OnPropertyChanged(nameof(RightListViewList));
        }
    }
    private List<PizzaListItem> _orderList = new List<PizzaListItem>();

    public string BuyButtonText
    {
        get => _buyButtonText; set
        {
            _buyButtonText = value;
            OnPropertyChanged(nameof(BuyButtonText));
        }
    }
    private string _buyButtonText;

    public void BasketButton(bool choice)
    {
        _selectedList = choice;
        OnPropertyChanged(nameof(BuyButtonVisibility));
        OnPropertyChanged(nameof(BasketRowSpan));
        OnPropertyChanged(nameof(LeftListViewList));
        OnPropertyChanged(nameof(RightListViewList));
    }

    public async void AddPizzaToBasket(string pizza)
    {
        this.PizzasInBasket.Add(pizza);
        OnPropertyChanged(nameof(PizzasInBasket));
    }

    public ICommand InitCommand { get; }
    public ICommand PlaceOrderCommand { get; }
    public ICommand BasketButtonCommand { get; }
    public ICommand LogoutCommand { get; }

    public HomePageViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        PlaceOrderCommand = new PlaceOrderCommand(_navigationStore);
        InitCommand = new InitCommand(_navigationStore);
        BasketButtonCommand = new BasketButtonCommand(_navigationStore);
        LogoutCommand = new LogoutCommand(_navigationStore);
        AllPizzas = new Dictionary<string, List<string>>();
        PizzasInBasket = new List<string>();

        Naam = UserInfo.Instance.UserName;
        BuyButtonText = "Order";

        InitCommand.Execute(null);
    }

    public void OnPropertyChange(object obj)
    {
        OnPropertyChanged(nameof(obj));
    }
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
    }
}