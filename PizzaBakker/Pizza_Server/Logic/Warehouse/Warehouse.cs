using Newtonsoft.Json;
using REI_Server.Logic.Connections;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REI_Server.Logic.Warehouse
{
    internal class Warehouse
    {
        private Dictionary<uint, WarehouseItem> _ingredients;

        private readonly string _fileName = $"ingredients";

        public Warehouse()
        {
            LoadFromFile();
        }

        public void AddIngredient(WarehouseItem item)
        {
            _ingredients.Add(item.Ingredient.Id, item);
        }

        public void SaveToFile()
        {

        }

        private void LoadFromFile()
        {
            _ingredients = IO.ReadObjectFromFile<Dictionary<uint, WarehouseItem>>(_fileName);

            if (_ingredients == null)
                _ingredients = new Dictionary<uint, WarehouseItem>();
        }
    }
}
