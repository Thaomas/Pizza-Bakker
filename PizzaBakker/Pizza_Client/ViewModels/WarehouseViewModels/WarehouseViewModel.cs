using Pizza_Client.Commands.WarehouseCommands;
using Pizza_Client.Stores;
using Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pizza_Client.ViewModels
{
    class WarehouseViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
        
        private string _NewIngredientName;
        public string NewIngredientName
        {
            get => _NewIngredientName;
            set
            {
                _NewIngredientName = value;
                OnPropertyChanged(nameof(NewIngredientName));
            }
        }


        private uint _newIngredientPrice;
        public uint NewIngredientPrice
        {
            get => _newIngredientPrice;
            set
            {
                _newIngredientPrice = value;
                OnPropertyChanged(nameof(IngredientPrice));
            }
        }


        private uint _NewIngredientAmount;
        public uint NewIngredientAmount
        {
            get => _NewIngredientAmount;
            set
            {
                _NewIngredientAmount = value;
                OnPropertyChanged(nameof(NewIngredientAmount));
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

        private WarehouseItem _selectedIngredient;
        public WarehouseItem SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;

                IngredientName = value.Ingredient.Name;
                IngredientPrice = value.Ingredient.Price.ToString();
                IngredientAmount = value.Count.ToString();

                OnPropertyChanged(nameof(SelectedIngredient));
            }
        }
       
        public ICommand AddIngredientCommand { get; }
        public ICommand ReloadListCommand { get; }
        public ICommand DeleteIngredientCommand { get; }

        public WarehouseViewModel(NavigationStore navigationStore)
        {

            _navigationStore = navigationStore;
            AllIngredients = new List<WarehouseItem>();

            AddIngredientCommand = new AddIngredientCommand(_navigationStore);
            ReloadListCommand = new ReloadListCommand(_navigationStore);
            DeleteIngredientCommand = new DeleteIngredientCommand(_navigationStore);

            //Load Ingredients
            Task.Run(() => {
                while (true)
                {
                    ReloadListCommand.Execute(null);
                    Thread.Sleep(3000);
                }
            });
            

        }
    }
}
