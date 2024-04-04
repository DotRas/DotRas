using System.Runtime.Serialization;

namespace ConsoleRunner.Model
{
    [DataContract(Name = "configuration", Namespace = "")]
    public class Configuration
    {
        [DataMember(Name = "entryName", IsRequired = true, Order = 1)]
        public string EntryName { get; set; }

        [DataMember(Name = "phoneBookPath", IsRequired = false, Order = 2)]
        public string PhoneBookPath { get; set; }

        [DataMember(Name = "username", IsRequired = false, Order = 3)]
        public string Username { get; set; }

        [DataMember(Name = "password", IsRequired = false, Order = 4)]
        public string Password { get; set; }
    }
}