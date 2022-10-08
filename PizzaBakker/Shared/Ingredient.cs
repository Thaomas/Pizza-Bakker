namespace Shared
{
    public class Ingredient
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} Name: {Name} Price: {Price}";
        }
    }
}