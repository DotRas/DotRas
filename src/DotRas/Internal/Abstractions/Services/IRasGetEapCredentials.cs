using System.Windows.Forms;
using DotRas.ExtensibleAuthentication;

namespace DotRas.Internal.Abstractions.Services
{
    internal interface IRasGetEapCredentials
    {
        EapCredential ForCurrentUser(string phoneBookPath, string entryName);

        EapCredential PromptUserForInformation(string phoneBookPath, string entryName, IWin32Window owner);
    }
}