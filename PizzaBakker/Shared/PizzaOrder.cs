using System;

namespace Shared
{
    public class PizzaOrder
    {
        public string Name { get; set; }
        public Guid OrderID { get; set; }
        public Tuple<Ingredient, int>[] Toppings { get; set; }

        public decimal Price
        {
            get
            {
                decimal total = 0m;
                foreach (var (ingredient, amount) in Toppings)
                {
                    total += ingredient.Price * amount;
                }
                return total;
            }
        }
    }
}
