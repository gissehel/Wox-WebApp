using System;

namespace Wox.WebApp.Core.Service
{
    public interface IFileReader : IDisposable
    {
        string ReadLine();
    }
}