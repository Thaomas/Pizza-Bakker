using Pizza_Client.Stores;
using Pizza_Client.ViewModels;
using Shared;

namespace Pizza_Client.Commands.WarehouseCommands
{
    public class AddIngredientCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private AddIngredientViewModel AddIngredientViewModel => (AddIngredientViewModel)_navigationStore.CurrentViewModel;

        public AddIngredientCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {

        }

        private void Callback(DataPacket packet)
        {

        }
    }
}