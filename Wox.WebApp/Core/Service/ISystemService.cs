namespace Wox.WebApp.Core.Service
{
    public interface ISystemService
    {
        void StartCommandLine(string command, string arguments);

        string ApplicationDataPath { get; }

        string GetExportPath();

        string GetUID();
    }
}