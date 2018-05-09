using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.WebApp.Core.Service
{
    public interface IFileGeneratorService
    {
        IFileGenerator CreateGenerator(string path);
    }
}