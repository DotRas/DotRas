using System;
using System.IO;
using DotRas.Internal.Abstractions.Primitives;

namespace DotRas.Internal.Infrastructure.Primitives
{
    internal class FileSystem : IFileSystem
    {
        public bool VerifyFileExists(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return File.Exists(fileName);
        }
    }
}