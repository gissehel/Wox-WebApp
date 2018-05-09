using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class FileGeneratorService : IFileGeneratorService
    {
        public IFileGenerator CreateGenerator(string path)
        {
            return new FileGenerator(path);
        }
    }
}