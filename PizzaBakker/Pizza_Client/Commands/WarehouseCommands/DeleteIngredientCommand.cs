using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Order;
using System;

namespace Pizza_Client.Commands.WarehouseCommands
{
    public class DeleteIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private HomepageViewModel _homepageViewModel => (HomepageViewModel)_navigationStore.CurrentViewModel;

        public DeleteIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<ChangeStatusPacket>()
            {
                type = PacketType.DELETE_INGREDIENT,
                senderID = connectionHandler.ID,
                data = new ChangeStatusPacket()
                {
                    orderStatus = _homepageViewModel.Status
                }
            }, DeleteIngredientCallback);
        }

        private void DeleteIngredientCallback(DataPacket obj)
        {
            throw new NotImplementedException();
        }
    }
}