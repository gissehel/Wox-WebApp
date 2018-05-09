using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.WebApp.Core.Service
{
    public interface IFileGenerator : IDisposable
    {
        IFileGenerator AddLine(string line);

        void Generate();
    }
}