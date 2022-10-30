using Customer_Client.Logic;
using Customer_Client.Stores;
using Customer_Client.UI_Element;
using Customer_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Customer_Client;
using System.Collections.Generic;

namespace Customer_Client.Commands;

public class InitCommand : CommandBase
{
    private readonly NavigationStore _navigationStore;
    private HomePageViewModel _mainViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;

    public InitCommand(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }
    public override void Execute(object parameter)
    {
        ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
        connectionHandler.SendData(new DataPacket<GetListRequestPacket>()
        {
            type = PacketType.GET_PIZZA_LIST,
            data = new GetListRequestPacket() 
            { 
            
            }
        }, InitCallback);
    }

    public void InitCallback(DataPacket packet)
    {
        GetListResponsePacket data = packet.GetData<GetListResponsePacket>();
        List<PizzaListItem> list = new List<PizzaListItem>();
        foreach (var item in data.pizzas)
            list.Add(new PizzaListItem(item.Key, item.Value, _navigationStore, true));
        _mainViewModel.PizzaListItems = list;
        _mainViewModel.AllPizzas = data.pizzas;
    }
}
