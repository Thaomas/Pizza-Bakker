using System;
using Customer_Client.Commands;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Customer_Client.ViewModels;

namespace Customer_Client.UI_Element
{
    public class PizzaListItem
    {
        public string Name { get; }
        public List<string> Ingredients { get; }
        public ICommand BuyPizza { get; }
    
        public PizzaListItem(string name, List<string> ingredients, HomePageViewModel homePageViewModel)
        {
            Name = name;
            Ingredients = ingredients;
            BuyPizza = new AddToBasketCommand(Name, homePageViewModel);
        }
    }
    
    public class AddToBasketCommand : CommandBase
    {
        private string pizza;
        private HomePageViewModel homepgaeViewModel;
        public AddToBasketCommand(string pizza, HomePageViewModel hpvm)
        {
            this.pizza = pizza;
            homepgaeViewModel = hpvm;
        }

        public override void Execute(object parameter)
        {
            //Uncomment this, CRASH 
            //homepgaeViewModel.PizzasInBasket.Add(pizza);
        }
    }
}
