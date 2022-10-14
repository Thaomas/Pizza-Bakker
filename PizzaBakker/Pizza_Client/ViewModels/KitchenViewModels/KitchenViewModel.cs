using Pizza_Client.Commands.KitchenCommands;
using Pizza_Client.Stores;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Pizza_Client.Commands.KitchenCommands;
using Pizza_Server.Logic;

namespace Pizza_Client.ViewModels
{
    class KitchenViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;

        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        private List<OrderStatus> _orderStatuses;
        public List<OrderStatus> OrderStatuses
        {
            get => _orderStatuses;
            set
            {
                _orderStatuses = value;
                OnPropertyChanged(nameof(OrderStatuses));
            }
        }

        private OrderStatus _selectedOrderStatus;

        public OrderStatus SelectedOrderStatus
        {
            get => _selectedOrderStatus;
            set
            {
                _selectedOrderStatus = value;
                OnPropertyChanged(nameof(SelectedOrderStatus));
            }
        }
        
        
        private List<PizzaOrder> _allOrders;


        private List<PizzaOrder> _incomingOrders;
        public List<PizzaOrder> IncomingOrders
        {
            get => _incomingOrders;  
            set
            {
                _incomingOrders = value;
                OnPropertyChanged(nameof(IncomingOrders));
            }
        }

        private List<string> _selectedOrderPizzas;
        public List<string> SelectedOrderPizzas
        {
            get => _selectedOrderPizzas;
            set
            {
                _selectedOrderPizzas = value;

                OnPropertyChanged(nameof(SelectedOrderPizzas));
            }
        }
        
        
        private PizzaOrder _selectedIngredient;
        public PizzaOrder SelectedIngredient
        {
            get => _selectedIngredient;
            set
            {
                _selectedIngredient = value;
                SelectedOrderPizzas = value.AllPizzas;
                OnPropertyChanged(nameof(SelectedIngredient));
            }
        }

        public ICommand PlaceOrderCommand { get; }

        public KitchenViewModel(NavigationStore navigationStore)
        {

            _navigationStore = navigationStore;
            OrderStatuses = Enum.GetValues<OrderStatus>().ToList();
            SelectedOrderStatus = OrderStatus.ORDERED;
            PlaceOrderCommand = new PlaceOrderCommand(_navigationStore);
            PlaceOrderCommand.Execute(null);
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