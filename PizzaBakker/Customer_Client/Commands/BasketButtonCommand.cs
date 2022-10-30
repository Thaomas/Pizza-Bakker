using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
using System.Collections.Generic;

namespace Customer_Client.Commands
{
    public class BasketButtonCommand : CommandBase
    {

        private readonly NavigationStore _navigationStore;
        private HomePageViewModel _mainViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;

        public BasketButtonCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            bool basket = (parameter as string).Equals("Basket");

            _mainViewModel.BasketButton(basket);
            if (basket)
                return;

            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<GetOrderHistoryPacket>
            {
                type = Shared.PacketType.GET_ORDER_HISTORY,
                data = new GetOrderHistoryPacket()
                {
                    customerID = UserInfo.Instance.customerID
                }
            }, OrderHistoryCallback);
        }

        private void OrderHistoryCallback(DataPacket obj)
        {
            GetOrderHistoryResponsePacket data = obj.GetData<GetOrderHistoryResponsePacket>();
            Dictionary<string, PizzaOrder> orders = new Dictionary<string, PizzaOrder>();
            data.orderHistory.ForEach(o => orders.Add(o.ToString(), o));
            _mainViewModel.OrderHistory = orders;

        }
    }
}
