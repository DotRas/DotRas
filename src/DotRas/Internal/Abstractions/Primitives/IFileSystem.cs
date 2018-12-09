namespace DotRas.Internal.Abstractions.Primitives
{
    internal interface IFileSystem
    {
        bool VerifyFileExists(string fileName);
    }
}