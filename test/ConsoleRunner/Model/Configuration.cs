using System.Runtime.Serialization;

namespace ConsoleRunner.Model
{
    [DataContract(Name = "configuration", Namespace = "")]
    public class Configuration
    {
        [DataMember(Name = "entryName", IsRequired = true, Order = 1)]
        public string EntryName { get; set; }

        [DataMember(Name = "phoneBookPath", IsRequired = true, Order = 2)]
        public string PhoneBookPath { get; set; }

        [DataMember(Name = "username", IsRequired = true, Order = 3)]
        public string Username { get; set; }

        [DataMember(Name = "password", IsRequired = true, Order = 4)]
        public string Password { get; set; }
    }
}