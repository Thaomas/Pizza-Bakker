namespace REI_Server.Logic.Warehouse
{
    public class Ingredient
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public decimal Price { get; set; }


        public string ToString()
        {
            return "ID: "+Id + ", Name: " + Name + " , Price:" + Price;
        }
    }
}