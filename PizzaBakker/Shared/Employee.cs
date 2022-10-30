namespace Shared
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
        public uint WorkId { get; set; }
        public string Password { get; set; }

        public Employee(string firstName, string lastName, uint workId, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            WorkId = workId;
            Password = password;
        }
    }
}