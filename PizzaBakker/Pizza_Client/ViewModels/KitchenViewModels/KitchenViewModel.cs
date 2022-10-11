using Pizza_Client.Commands.WarehouseCommands;
using Pizza_Client.Stores;
using Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Pizza_Client.Commands.KitchenCommands;

namespace Pizza_Client.ViewModels
{
    class KitchenViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


        private string _ingredientPrice;

        public string IngredientPrice
        {
            get => _ingredientPrice;
            set
            {
                _ingredientPrice = value;
                OnPropertyChanged(nameof(IngredientPrice));
            }
        }


        private string _ingredientAmount;

        public string IngredientAmount
        {
            get => _ingredientAmount;
            set
            {
                _ingredientAmount = value;
                OnPropertyChanged(nameof(IngredientAmount));
            }
        }

        private List<WarehouseItem> _allIngredients;

        public List<WarehouseItem> AllIngredients
        {
            get => _allIngredients;
            set
            {
                _allIngredients = value;
                OnPropertyChanged(nameof(AllIngredients));
            }
        }
        
        private List<string> _incomingOrders;

        public List<string> IncomingOrders {
            get => _incomingOrders;
            set
            {
                _incomingOrders = value;
                OnPropertyChanged(nameof(IncomingOrders));
            }
        }

        private WarehouseItem _selectedIngredient;

        public WarehouseItem SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;

                OnPropertyChanged(nameof(SelectedIngredient));
            }
        }

        public ICommand PlaceOrderCommand { get; }
        
        public KitchenViewModel(NavigationStore navigationStore)
        {

            _navigationStore = navigationStore;
            AllIngredients = new List<WarehouseItem>();

            PlaceOrderCommand = new PlaceOrderCommand(_navigationStore);

            //Load Ingredients for all the connected clients every 3-Seconds
            
            /*Task.Run(() =>
            {
                while (true)
                {
                    ReloadListCommand.Execute(null);
                    Thread.Sleep(3000);
                }
            });*/
        }
    }
}