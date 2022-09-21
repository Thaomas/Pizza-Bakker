using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class JsonFile
    {
        public int StatusCode { get; set; }
        public int OppCode { get; set; }
        public uint ID { get; set; }
        public JsonData Data { get; set; }
    }

    public class JsonData
    {
        public uint AutenticationID { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public bool IsFileAdded { get; set; }
        public bool AccesToFile { get; set; }
        public Employee Employee { get; set; }
        public List<Employee> EmployeeInList { get; set; }
        public int EmployeeStatus { get; set; }
        public PrivacyLevel PrivacyLevel { get; set; }
    }
}
