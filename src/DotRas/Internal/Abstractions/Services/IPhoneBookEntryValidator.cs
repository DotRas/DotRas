namespace DotRas.Internal.Abstractions.Services
{
    internal interface IPhoneBookEntryValidator
    {
        bool VerifyEntryExists(string entryName, string phoneBookPath);
    }
}