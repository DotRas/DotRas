using System;
using DotRas.Internal.Abstractions.Providers;
using DotRas.Win32;
using static DotRas.Win32.WinError;

namespace DotRas.Internal.Providers
{
    internal class PhoneBookEntryValidator : IPhoneBookEntryValidator
    {
        private readonly IRasApi32 api;

        public PhoneBookEntryValidator(IRasApi32 api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public bool VerifyEntryExists(string entryName, string phoneBookPath)
        {
            if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }
            else if (string.IsNullOrWhiteSpace(phoneBookPath))
            {
                throw new ArgumentNullException(nameof(phoneBookPath));
            }

            var ret = api.RasValidateEntryName(phoneBookPath, entryName);
            return ret == ERROR_ALREADY_EXISTS;
        }
    }
}