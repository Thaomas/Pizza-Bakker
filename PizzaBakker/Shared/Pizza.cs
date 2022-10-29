using System.Collections.Generic;

namespace Shared
{
    public class Pizza
    {
        public string Name;
        public List<uint> Ingredients;
        public override string ToString()
        {
            return Name;
        }
    }
}
