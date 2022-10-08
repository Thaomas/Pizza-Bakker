using Pizza_Client.Stores;
using Pizza_Client.ViewModels;
using Shared;

namespace Pizza_Client.Commands.WarehouseCommands
{
    public class AddIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private WarehouseViewModel WarehouseViewModel => (WarehouseViewModel)_navigationStore.CurrentViewModel;

        public AddIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            WarehouseViewModel.Debug = "miauwww";

            /*ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(Callback, new DataPacket<AddIngredientPacket>()
            {
                type = PacketType.ADD_INGREDIENT,
                senderID = connectionHandler.ID,
                
            });*/
        }

        private void Callback(DataPacket packet)
        {
            WarehouseViewModel.Debug = "miauwww";
        }
    }
}