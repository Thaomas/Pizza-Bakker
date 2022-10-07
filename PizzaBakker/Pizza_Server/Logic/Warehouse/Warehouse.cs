using Newtonsoft.Json;
using REI_Server.Logic.Connections;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace REI_Server.Logic.Warehouse
{
    public class Warehouse
    {
        public List<WarehouseItem> _ingredients = new();

        private readonly string _fileName = $"ingredients";


        public Warehouse()
        {
            LoadFromFile();
        }

        /*public void startSavingThread()
        {
            new Thread(SavingLoop).Start();
        }

        private void SavingLoop()
        {
            while (true)
            {
                Thread.Sleep(3000);

                string serializeData = JsonConvert.SerializeObject(_ingredients, Formatting.Indented);
                IO.WriteFile("Warehouse.json", serializeData);

                Console.WriteLine("saving loop !");
            }
        }*/

        public void orderPizza(List<string> orderPizza)
        {
            bool _orderComplete = true;
            List<string> _outOfStockIngredients = new();

            foreach (string singleIngedient in orderPizza)
            {
                if (!decrementIngredient(singleIngedient))
                {
                    _orderComplete = false;
                    if (!_outOfStockIngredients.Contains(singleIngedient))
                    {
                        _outOfStockIngredients.Add(singleIngedient);
                    }
                    
                }                              
            }

            if (_orderComplete)
            {
                Console.WriteLine("Bestelling is klaar gemaakt!");
            }
            else
            {
                Console.WriteLine("De bestelling kon niet afgemaatk worder ern missen een aantal ingredienten zoals: ");

                foreach (string singleIngedient in _outOfStockIngredients)
                {
                    Console.WriteLine(singleIngedient);
                }
            }
        }

        //WERKT!!
        public bool decrementIngredient(string singleIngredient)
        {
            
            WarehouseItem retrievedIngredient = _ingredients.First(name => name.Ingredient.Name.Equals(singleIngredient));

            if (retrievedIngredient.Count > 0)
            {
                retrievedIngredient.Count -= 1;

                _ingredients.Remove(retrievedIngredient);
                _ingredients.Add(retrievedIngredient);
                 return true;
            }
            else
            {
                Console.WriteLine("Het product: " + retrievedIngredient.Ingredient.Name + " is uitverkocht!");
                return false;
            }                
        }
                    

        //Werkt!
        public void AddIngredient(WarehouseItem item)
        {
            bool isExist = _ingredients.Exists(ti => ti.Ingredient.Name.Equals(item.Ingredient.Name));

            if (isExist)
            {
                Console.WriteLine("bestaat al sorry");
                return;
            }
            _ingredients.Add(item);
        }

        //WERKT!
        public void DeleteIngredient(string item)
        {
            try
            {
                WarehouseItem deletedItem = _ingredients.First(name => name.Ingredient.Name.Equals(item));
                _ingredients.Remove(deletedItem);
            } catch (Exception e)
            {
                Console.WriteLine(e + "Naam bestaat niet, dus hebben wij het niet kunnen verwijderen!!");
            }
        }

        //WERKT!
        public WarehouseItem RetrieveIngredient(string item)
        {

            WarehouseItem foundIngredient = null;

            try {
                foundIngredient = _ingredients.First(name => name.Ingredient.Name.Equals(item));
            } catch (Exception e) {
                Console.WriteLine(e + "Naam bestaat niet, dus hebben wij het niet kunnen verwijderen!!");
            }

            return foundIngredient;
        }


        public void LoadFromFile() {          

            _ingredients = JsonConvert.DeserializeObject<List<WarehouseItem>>(IO.ReadFile("SaveData\\Warehouse.json"));

            if (_ingredients == null) {
                Console.WriteLine("Geen ingredienten beschikbaar!");
            }
                        
        }
    }
}
