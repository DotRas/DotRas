using System;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;
using static DotRas.Internal.Interop.WinError;

namespace DotRas.Internal.Services.PhoneBooks
{
    internal class PhoneBookEntryNameValidationService : IPhoneBookEntryValidator
    {
        private readonly IRasApi32 api;

        public PhoneBookEntryNameValidationService(IRasApi32 api)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public bool VerifyEntryExists(string entryName, string phoneBookPath)
        {
            if (string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException(nameof(entryName));
            }

            var ret = api.RasValidateEntryName(phoneBookPath, entryName);
            return ret == ERROR_ALREADY_EXISTS;
        }
    }
}