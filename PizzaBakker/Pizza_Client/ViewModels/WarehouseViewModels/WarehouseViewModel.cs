using Pizza_Client.Commands.WarehouseCommands;
using Pizza_Client.Stores;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Pizza_Client.ViewModels
{
    class WarehouseViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;


        private string _debug;
        public string Debug
        {
            get => _debug;
            set
            {
                _debug = value;
                OnPropertyChanged(nameof(Debug));
            }
        }

        private string _ingredientName;
        public string IngredientName
        {
            get => _ingredientName;
            set
            {
                _ingredientName = value;
                OnPropertyChanged(nameof(IngredientName));
            }
        }


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

        private Dictionary<string, WarehouseItem> _allIngredients;
        public Dictionary<string, WarehouseItem> AllIngredients
        {
            get => _allIngredients;
            set
            {
                _allIngredients = value;
                AllIngredientKeys = _allIngredients.Keys.ToList();
            }
        }

        private List<string> _allIngredientKeys;
        public List<string> AllIngredientKeys
        {
            get => _allIngredients.Keys.ToList();
            set
            {
                _allIngredientKeys = value;
                OnPropertyChanged(nameof(AllIngredientKeys));
            }
        }

        private string _selectedIngredient;
        public string SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;

                WarehouseItem item = _allIngredients[value];

                IngredientName = item.Ingredient.Name;
                IngredientPrice = item.Ingredient.Price.ToString();
                IngredientAmount = item.Count.ToString();

                OnPropertyChanged(nameof(SelectedIngredient));
            }
        }

        public ICommand AddIngredientCommand { get; }
        public ICommand ReloadListCommand { get; }
        public ICommand DeleteIngredientCommand { get; }

        public WarehouseViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            AllIngredients = new Dictionary<string, WarehouseItem>();

            AddIngredientCommand = new AddIngredientCommand(_navigationStore);
            ReloadListCommand = new ReloadListCommand(_navigationStore);
            DeleteIngredientCommand = new DeleteIngredientCommand(_navigationStore);

            //Load Ingredients
            ReloadListCommand.Execute(null);

        }
    }
}
