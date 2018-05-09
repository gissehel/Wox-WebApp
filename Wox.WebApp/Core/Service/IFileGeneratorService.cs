namespace Wox.WebApp.Core.Service
{
    public interface IFileGeneratorService
    {
        IFileGenerator CreateGenerator(string path);
    }
}