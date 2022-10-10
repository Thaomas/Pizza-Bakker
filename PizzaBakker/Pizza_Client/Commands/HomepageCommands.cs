using Employee_Client.Stores;
using Employee_Client.Util;
using Employee_Client.ViewModels;
using Shared;
using Shared.Packet;
using Shared.Packet.Order;
using System;

namespace Employee_Client.Commands
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
