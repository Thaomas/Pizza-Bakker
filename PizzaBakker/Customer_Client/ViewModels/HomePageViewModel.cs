using Customer_Client.Commands;
using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.UI_Element;
using Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

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

    private int _basketRowSpan = 1;
    public int BasketRowSpan
    {
        get => (_selectedList) ? 1 : 2;
    }

    private Visibility _buyButtonVisibility = Visibility.Visible;
    public Visibility BuyButtonVisibility
    {
        get => (_selectedList) ? Visibility.Visible : Visibility.Hidden;
    }
    private bool _selectedList = true;
    public List<string> RightListViewList { get => _selectedList ? PizzasInBasket : OrderHistoryKeys; }
    
    private List<string> _pizzasInBasket;
    public List<string> PizzasInBasket
    {
        get => _pizzasInBasket;
        set
        {
            _pizzasInBasket = value;
            OnPropertyChanged(nameof(RightListViewList));
        }
    }

    private Dictionary<string, PizzaOrder> _orderHistory = new Dictionary<string, PizzaOrder>();
    public Dictionary<string, PizzaOrder> OrderHistory
    {
        get => _orderHistory;
        set
        {
            _orderHistory = value;
            OnPropertyChanged(nameof(RightListViewList));
        }
    }
    public List<string> OrderHistoryKeys
    {
        get => _orderHistory.Keys.ToList();
    }

    private string _selectedOrder;
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
                if(AllPizzas.TryGetValue(p, out ingredients))
                    _orderList.Add(new PizzaListItem(p, ingredients, _navigationStore, false));
                else
                    _orderList.Add(new PizzaListItem(p, new List<string>() { "Error, I dont exist"}, _navigationStore, false));
            });
            
            OnPropertyChanged(nameof(LeftListViewList));
        }
    }

    public Dictionary<string ,List<string>> AllPizzas { get; set; }

    private List<PizzaListItem> _leftListViewList = new List<PizzaListItem>();
    public List<PizzaListItem> LeftListViewList { get => _selectedList ? PizzaListItems : OrderListItems ;}

    private List<PizzaListItem> _pizzaList = new List<PizzaListItem>();
    public List<PizzaListItem> PizzaListItems
    {
        get => _pizzaList; set
        {
            _pizzaList = value;
            OnPropertyChanged(nameof(PizzaListItems));
        }
    }
    
    private List<PizzaListItem> _orderList = new List<PizzaListItem>();
    public List<PizzaListItem> OrderListItems
    {
        get => _pizzaList; set
        {
            _pizzaList = value;
            OnPropertyChanged(nameof(RightListViewList));
        }
    }
    public void BasketButton(bool choice)
    {
        _selectedList = choice;
        OnPropertyChanged(nameof(BuyButtonVisibility));
        OnPropertyChanged(nameof(BasketRowSpan));
        OnPropertyChanged(nameof(LeftListViewList));
        OnPropertyChanged(nameof(RightListViewList));
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(_navigationStore.CurrentViewModel));
    }

    public async void AddPizzaToBasket(string pizza)
    {
        this.PizzasInBasket.Add(pizza);
        OnPropertyChanged(nameof(PizzasInBasket));
    }


    public ICommand InitCommand { get; }
    public ICommand PlaceOrderCommand { get; }
    public ICommand BasketButtonCommand { get; }
    public HomePageViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        
        PlaceOrderCommand = new PlaceOrderCommand(_navigationStore);
        InitCommand = new InitCommand(_navigationStore);
        BasketButtonCommand = new BasketButtonCommand(_navigationStore);

        AllPizzas = new Dictionary<string, List<string>>();
        PizzasInBasket = new List<string>();
        Naam = UserInfo.Instance.UserName;
        InitCommand.Execute(null);
    }
}