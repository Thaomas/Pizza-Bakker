using Customer_Client.Commands;
using System.Collections.Generic;
using System.Windows.Input;

namespace Customer_Client.UI_Element
{
    public class PizzaListItem
    {
        public string Name { get; }
        public List<string> Ingredients { get; }
        public ICommand BuyPizza;

        public PizzaListItem(string name, List<string> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
            BuyPizza = new AddToBasketCommand(Name);
        }


    }
    public class AddToBasketCommand : CommandBase
    {
        private string pizza;

        public AddToBasketCommand(string pizza)
        {
            this.pizza = pizza;
        }

        public override void Execute(object parameter)
        {


        }
    }
}
