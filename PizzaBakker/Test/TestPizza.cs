using NUnit.Framework;
using Shared;
using System.Collections.Generic;

namespace Test
{
    public class TestPizza
    {
        private Pizza _pizzaSalami;

        private Pizza _pizzaPollo;

        [SetUp]
        public void Setup()
        {
            _pizzaSalami = new Pizza()
            {
                Name = "Pizza Salami",
                Ingredients = new List<uint>() { 1, 2, 3, 4 }
            };

            _pizzaPollo = new Pizza()
            {
                Name = "Pizza Pollo",
                Ingredients = new List<uint>() { 1, 2, 3, 6, 7 }
            };
        }

        [Test]
        public void Test_Pizza_Pollo_Name()
        {
            Assert.AreEqual(_pizzaPollo.Name, "Pizza Pollo");
        }

        [Test]
        public void Test_Pizza_Salami_Name()
        {
            Assert.AreEqual(_pizzaSalami.Name, "Pizza Salami");
        }

        [Test]
        public void Test_Ingredient_More_Than_Pizza_Salami()
        {
            Assert.Greater(_pizzaPollo.Ingredients.Count, _pizzaSalami.Ingredients.Count);
        }
    }
}