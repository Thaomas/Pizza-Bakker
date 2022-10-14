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

        
        
        
        private List<PizzaOrder> _allOrders;

        public List<PizzaOrder> AllOrders
        {
            set
            {
                _allOrders = value;
                OnPropertyChanged(nameof(IncomingOrders));
                OnPropertyChanged(nameof(InProgressOrders));
                OnPropertyChanged(nameof(DeliveryOrders));
                OnPropertyChanged(nameof(DeliveredOrders));
            }
        }

        public List<PizzaOrder> IncomingOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.ORDERED)).ToList<PizzaOrder>();
        public List<PizzaOrder> InProgressOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.IN_PROGRESS)).ToList<PizzaOrder>();
        public List<PizzaOrder> DeliveryOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.DELIVERING)).ToList<PizzaOrder>();
        public List<PizzaOrder> DeliveredOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.DELIVERED)).ToList<PizzaOrder>();

        private PizzaOrder _selectedOrder;
        public PizzaOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                SelectedOrderPizzas = value.AllPizzas;
                SelectedOrderStatus = value.Status;
                OnPropertyChanged(nameof(SelectedOrder));
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

        private OrderStatus _selectedOrderStatus;
        public OrderStatus SelectedOrderStatus
        {
            get => _selectedOrderStatus;
            set
            {
                _selectedOrderStatus = value;
                OnPropertyChanged(nameof(SelectedOrderStatus));

                if (_selectedOrderStatus == null || SelectedOrder == null|| SelectedOrder?.Status == value)
                    return;
                SelectedOrder.Status = value;

                OnPropertyChanged(nameof(IncomingOrders));
                OnPropertyChanged(nameof(InProgressOrders));
                OnPropertyChanged(nameof(DeliveryOrders));
                OnPropertyChanged(nameof(DeliveredOrders)); 
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