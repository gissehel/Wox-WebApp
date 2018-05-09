using System.IO;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class FileReaderService : IFileReaderService
    {
        public bool FileExists(string path) => File.Exists(path);

        public IFileReader Read(string path)
        {
            return new FileReader(path);
        }
    }
}