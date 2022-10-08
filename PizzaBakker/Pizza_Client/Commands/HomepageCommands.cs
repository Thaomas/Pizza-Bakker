using Pizza_Client.Stores;
using Pizza_Client.Util;
using Pizza_Client.ViewModels;
using Shared;
using Shared.Order;
using System;

namespace Pizza_Client.Commands
{
    public class ChangeStatusCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private HomepageViewModel _homepageViewModel => (HomepageViewModel)_navigationStore.CurrentViewModel;
        public ChangeStatusCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(new DataPacket<ChangeStatusPacket>()
            {
                type = PacketType.CHANGE_STATUS,
                data = new ChangeStatusPacket()
                {
                    orderStatus = _homepageViewModel.Status
                }
            }, HomepageCallback);
        }

        private void HomepageCallback(DataPacket obj)
        {
            throw new NotImplementedException();
        }
    }
}
