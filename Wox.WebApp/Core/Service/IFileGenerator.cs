using System;

namespace Wox.WebApp.Core.Service
{
    public interface IFileGenerator : IDisposable
    {
        IFileGenerator AddLine(string line);

        void Generate();
    }
}