using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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