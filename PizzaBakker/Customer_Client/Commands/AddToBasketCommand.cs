using Customer_Client.Stores;
using Customer_Client.Util;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
using Shared.Packet.Warehouse;

namespace Customer_Client.Commands;

public class AddToBasketCommand: CommandBase
{
    private readonly NavigationStore _navigationStore;
    private HomePageViewModel _addIngredientViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;

    public AddToBasketCommand(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }

    public override void Execute(object parameter)
    {
        ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
        connectionHandler.SendData(new DataPacket<AddToBasketRequestPacket>()
        {
            type = PacketType.ADD_TO_BASKET,
            data = new AddToBasketRequestPacket()
            {
               pizzaName = _addIngredientViewModel.SelectedPizza.Name
            }
        }, LoginCallback); 
    }

    public void LoginCallback(DataPacket packet)
    {
        AddToBasketResponsePacket data = packet.GetData<AddToBasketResponsePacket>();

        _addIngredientViewModel.PizzasInBasket = data.pizzas;
    }
}