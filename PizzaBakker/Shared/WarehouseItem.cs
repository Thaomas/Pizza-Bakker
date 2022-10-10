namespace Shared
{
    public class WarehouseItem
    {
        public Ingredient Ingredient { get; set; }
        public uint Count { get; set; }
        public override string ToString()
        {
            return $"{Ingredient.Name} | {Ingredient.Price} | {Count}";
        }
    }
}