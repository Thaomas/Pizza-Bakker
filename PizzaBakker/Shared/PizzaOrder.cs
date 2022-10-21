using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Shared
{
    public class PizzaOrder
    {
        public List<string> AllPizzas { get; set; }
        public uint OrderId { get; set; }
        public Guid OrderId2 { get; set; }
        public Guid CustomerID2 { get; set; }
        public uint CustomerID { get; set; }
        public OrderStatus Status { get; set; }


        public PizzaOrder()
        {
            AllPizzas = new();
        }

        public string Title => $"Order {OrderId.ToString()}";

        public override string ToString()
        {
            return $"Order {OrderId.ToString()} | Pizza's: {AllPizzas.Count}";

        }
        //TODO DEBUG CODE
        public PizzaOrder Clone() => JsonConvert.DeserializeObject<PizzaOrder>(JsonConvert.SerializeObject(this));
    }
}