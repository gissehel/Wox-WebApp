namespace Wox.WebApp.Core.Service
{
    public interface IWoxContextService
    {
        void ChangeQuery(string query);

        string ActionKeyword { get; }

        string Seperater { get; }
    }
}