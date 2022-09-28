using REI.Stores;
using REI.Util;
using REI.ViewModels;
using Shared;
using Shared.Order;
using System;

namespace REI.Commands
{
    public class ChangeStatusCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public ChangeStatusCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            ConnectionHandler connectionHandler = ConnectionHandler.GetInstance();
            connectionHandler.SendData(Callback, new DataPacket<ChangeStatusPacket>()
            {
                type = PacketType.CHANGE_STATUS,
                senderID = connectionHandler.ID,
                data = new ChangeStatusPacket()
                {
                    orderStatus = ((HomepageViewModel)_navigationStore.CurrentViewModel).Status
                }
            }) ;
        }

        private void Callback(DataPacket obj)
        {
            throw new NotImplementedException();
        }
    }
}
