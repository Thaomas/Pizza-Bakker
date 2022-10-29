using System;
using System.Collections.Generic;
using System.Text;

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
