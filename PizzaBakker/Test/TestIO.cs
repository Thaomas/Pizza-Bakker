using Newtonsoft.Json;
using NUnit.Framework;
using Pizza_Server.Logic.Connections;
using Pizza_Server.Models;
using System.Collections.Generic;

namespace Test
{
    class TestIO
    {
        public Note singleNote = new();
        public Dictionary<string, Note> retrievednote = new();
        public Note goodNote;

        [SetUp]
        public void Setup()
        {
            Note note = new();
            string retieveNotes = IO.ReadFile("TestData\\TestJsonNotes.json");

            retrievednote = JsonConvert.DeserializeObject<Dictionary<string, Note>>(retieveNotes);

            if (retrievednote.TryGetValue("new file", out note))
            {
                singleNote = retrievednote["new file"];
            }


            goodNote = new()
            {
                Title = "new file",
                Content = "dit is context van de file"
            };
        }

        [Test]
        public void Test1()
        {


            Assert.AreEqual(goodNote.Title, singleNote.Title);
        }
    }
}
