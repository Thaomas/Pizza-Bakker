
using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Kitchen;
using System.Threading.Tasks;

namespace Customer_Client.Commands
{
    public class PlaceOrderCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private HomePageViewModel _homePageViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;

        public PlaceOrderCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {

            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<PlaceOrderRequestPacket>()
            {
                type = PacketType.PLACE_ORDER,
                data = new PlaceOrderRequestPacket()
                {
                    pizzaOrder = _homePageViewModel.PizzasInBasket,
                    customerID = UserInfo.Instance.customerID
                }
            }, PlaceOrderCallback);
            _homePageViewModel.PizzasInBasket.Clear();
            _homePageViewModel.OnPropertyChange(_homePageViewModel.PizzasInBasket);
        }

        private async void PlaceOrderCallback(DataPacket obj)
        {
            PlaceOrderResponsePacket data = obj.GetData<PlaceOrderResponsePacket>();

            string old = _homePageViewModel.BuyButtonText;
            _homePageViewModel.BuyButtonText = data.statusCode.Equals(StatusCode.OK) ? "Ordered!" : "Order failed";
            await Task.Delay(1000);
            _homePageViewModel.BuyButtonText = old;
        }
    }
}