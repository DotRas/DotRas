using System.Runtime.Serialization;

namespace ConsoleRunner.Model
{
    [DataContract(Name = "configuration", Namespace = "")]
    public class Configuration
    {
        [DataMember(Name = "entryName", IsRequired = true)]
        public string EntryName { get; set; }

        [DataMember(Name = "username", IsRequired = true)]
        public string Username { get; set; }

        [DataMember(Name = "password", IsRequired = true)]
        public string Password { get; set; }
    }
}