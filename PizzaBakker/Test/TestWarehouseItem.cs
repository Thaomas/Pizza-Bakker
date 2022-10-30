using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestPlatform;
using Shared;

namespace Test
{
    public class TestEmployee
    {
        private WarehouseItem deeg;

        private WarehouseItem kaas;
        
        [SetUp]
        public void Setup()
        {
            deeg = new WarehouseItem(){
                Ingredient = new Ingredient()
                {
                    Id = 1,
                    Name = "Deeg",
                    Price = 1
                },
                Count = 22
            };
            
            kaas = new WarehouseItem(){
                Ingredient = new Ingredient()
                {
                    Id = 2,
                    Name = "Kaas",
                    Price = 3
                },
                Count = 15
            };
            
         
        }

        [Test]
        public void Test_Warehouse_Item_Deeg_Count()
        {
            Assert.AreEqual(deeg.Count,22);
        }
        
        [Test]
        public void Test_Warehouse_Item_Deeg_Name()
        {
            Assert.AreEqual(deeg.Ingredient.Name,"Deeg");
        }
        
        [Test]
        public void Test_Warehouse_Item_Kaas_Name()
        {
            Assert.AreEqual(kaas.Ingredient.Name,"Kaas");
        }
        
        [Test]
        public void Test_Warehouse_Item_Price_compare()
        {
            Assert.Greater(kaas.Ingredient.Price, deeg.Ingredient.Price);
        }
    }
}