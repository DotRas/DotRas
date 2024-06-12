using DotRas.Internal.Abstractions.Primitives;
using System;
using System.IO;

namespace DotRas.Internal.Infrastructure.Primitives {
    internal class FileSystem : IFileSystem {
        public bool VerifyFileExists(string fileName) {
            return string.IsNullOrWhiteSpace(fileName) ? throw new ArgumentNullException(nameof(fileName)) : File.Exists(fileName);
        }
    }
}
