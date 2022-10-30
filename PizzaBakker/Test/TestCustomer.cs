using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using Shared;

namespace Test
{
    public class TestCustomer
    {
        private Customer setUpCustomer1;
        private Customer setUpCustomer2;
        
        [SetUp]
        public void Setup()
        {
            setUpCustomer1 = new Customer("kees", "kabeljauwsteeg");
            setUpCustomer2 = new Customer("sander", "zalmstraat");
            
            setUpCustomer2.OrderHistory.Add(new PizzaOrder());
            setUpCustomer2.OrderHistory.Add(new PizzaOrder());
            setUpCustomer2.OrderHistory.Add(new PizzaOrder());
        }

        [Test]
        public void Test_Customer_1_Name()
        {
            Assert.AreEqual(setUpCustomer1.Name, "kees");
        }
        
        [Test]
        public void Test_Customer_1_Address()
        {
            Assert.AreEqual(setUpCustomer1.Address,"kabeljauwsteeg");
        }
        
        [Test]
        public void Test_Customer_1_List_Filled()
        {
            Assert.Greater(setUpCustomer2.OrderHistory.Count,2);
        }
    }
}