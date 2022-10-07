using REI.Commands;
using REI.Stores;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using REI.Util;
using Shared.Login;
using Shared.Warehouse;

namespace REI.ViewModels
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
        
        private string _selectedIngredient;
        public string SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {

                Debug = value;
                _selectedIngredient = value;
                OnPropertyChanged(nameof(SelectedIngredient));
            }
        }

        public ICommand AddIngredientCommand { get; }
        public ICommand ReloadListCommand { get; }
        public ICommand DeleteIngredientCommand { get; }

        public WarehouseViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            Task.Run(() =>
            {
                ConnectionHandler ch = ConnectionHandler.GetInstance();

                ch.SendData(packet => { AllIngredients = packet.GetData<GetListResponsePacket>().allItems; },
                    new DataPacket<GetListRequestPacket>
                    {
                        type = PacketType.GET_LIST,
                        data = new GetListRequestPacket() { }
                    });
            });

            AddIngredientCommand = new AddIngredientCommand(_navigationStore);
            ReloadListCommand = new ReloadListCommand(_navigationStore);
            DeleteIngredientCommand = new DeleteIngredientCommand(_navigationStore);
        }
    }
}
