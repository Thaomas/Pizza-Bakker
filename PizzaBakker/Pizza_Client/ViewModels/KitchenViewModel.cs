using Employee_Client.Commands.KitchenCommands;
using Employee_Client.Stores;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Employee_Client.ViewModels
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

        public List<PizzaOrder> IncomingOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.ORDERED)).ToList();
        public PizzaOrder SelectedIncomingOrders
        {
            get => SelectedOrder;
            set => SelectedOrder = value;
        }
        public List<PizzaOrder> InProgressOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.PREPARING)).ToList();
        public PizzaOrder SelectedInProgressOrders
        {
            get => SelectedOrder;
            set => SelectedOrder = value;
        }
        public List<PizzaOrder> DeliveryOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.DELIVERING)).ToList();
        public PizzaOrder SelectedDeliveryOrders
        {
            get => SelectedOrder;
            set => SelectedOrder = value;
        }
        public List<PizzaOrder> DeliveredOrders => _allOrders.Where(p => p.Status.Equals(OrderStatus.DELIVERED)).ToList();
        public PizzaOrder SelectedDeliveredOrders
        {
            get => SelectedOrder;
            set => SelectedOrder = value;
        }

        private PizzaOrder _selectedOrder;
        public PizzaOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (value == null)
                    return;

                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedIncomingOrders));
                OnPropertyChanged(nameof(SelectedInProgressOrders));
                OnPropertyChanged(nameof(SelectedDeliveryOrders));
                OnPropertyChanged(nameof(SelectedDeliveredOrders));

                SelectedOrderDetails = value.AllPizzas;
                value.AllPizzas.ForEach(s => Trace.WriteLine(s));
                SelectedOrderStatus = value.Status;
                SelectedOrderTitle = value.Title;
            }
        }

        private List<string> _selectedOrderDetails;
        public List<string> SelectedOrderDetails
        {
            get => _selectedOrderDetails;
            set
            {
                _selectedOrderDetails = value;
                OnPropertyChanged(nameof(SelectedOrderDetails));
            }
        }

        private string _selectedOrderTitle;
        public string SelectedOrderTitle
        {
            get => _selectedOrderTitle;
            set
            {
                _selectedOrderTitle = value;
                OnPropertyChanged(nameof(SelectedOrderTitle));
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

                if (SelectedOrder == null || SelectedOrder?.Status == value)
                    return;

                SelectedOrder.Status = value;

                ChangeStatusOrderCommand.Execute(SelectedOrder);

                OnPropertyChanged(nameof(IncomingOrders));
                OnPropertyChanged(nameof(InProgressOrders));
                OnPropertyChanged(nameof(DeliveryOrders));
                OnPropertyChanged(nameof(DeliveredOrders));
            }
        }

        public ICommand ChangeStatusOrderCommand { get; }
        public ICommand CheckOrderListCommand { get; }

        public KitchenViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            OrderStatuses = Enum.GetValues<OrderStatus>().ToList();
            SelectedOrderStatus = OrderStatus.ORDERED;
            SelectedOrderTitle = "No Order Selected";
            AllOrders = new List<PizzaOrder>();

            ChangeStatusOrderCommand = new ChangeStatusOrderCommand();
            CheckOrderListCommand = new CheckOrderListCommand(navigationStore);

            Task.Run(() =>
            {
                while (true)
                {
                    CheckOrderListCommand.Execute(null);
                    Thread.Sleep(2000);
                }
            });
        }



    }
}