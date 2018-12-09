namespace DotRas.Internal.Abstractions.Providers
{
    internal interface IPhoneBookEntryValidator
    {
        bool VerifyEntryExists(string entryName, string phoneBookPath);
    }
}