using System;
using Customer_Client.Commands;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Customer_Client.ViewModels;
using Customer_Client.Stores;

namespace Customer_Client.UI_Element
{
    public class PizzaListItem
    {
        public string Name { get; }
        public List<string> Ingredients { get; }
        public ICommand BuyPizza { get; }
    
        public PizzaListItem(string name, List<string> ingredients, NavigationStore navStore)
        {
            Name = name;
            Ingredients = ingredients;
            BuyPizza = new AddToBasketCommand(Name, navStore);
        }
    }
    
    public class AddToBasketCommand : CommandBase
    {
        private string pizza;
        private readonly NavigationStore _navigationStore;
        private HomePageViewModel _mainViewModel => (HomePageViewModel)_navigationStore.CurrentViewModel;
        public AddToBasketCommand(string pizza, NavigationStore navStore)
        {
            this.pizza = pizza;
            _navigationStore = navStore;
        }

        public override void Execute(object parameter)
        {

            _mainViewModel.AddPizzaToBasket(pizza);
        }
    }
}
