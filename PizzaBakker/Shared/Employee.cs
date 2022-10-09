using System;

namespace Shared
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
        public uint WorkId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public PrivacyLevel PrivacyLevel { get; set; }

        public Employee(string firstName, string lastName, uint workId, DateTime? birthday, string department, string password, PrivacyLevel privacyLevel)
        {
            FirstName = firstName;
            LastName = lastName;
            WorkId = workId;
            Birthday = birthday;
            Department = department;
            Password = password;
            PrivacyLevel = privacyLevel;
        }
    }
}
