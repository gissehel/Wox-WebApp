using System;
using System.Collections.Generic;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Mock.Service
{
    public class FileGeneratorMock : IFileGenerator
    {
        public List<string> Lines { get; private set; } = new List<string>();
        public string Path { get; private set; }
        private bool Opened { get; set; } = false;

        public FileGeneratorMock(string path)
        {
            Path = path;
            Opened = true;
        }

        public IFileGenerator AddLine(string line)
        {
            if (Opened)
            {
                Lines.Add(line);
            }
            return this;
        }

        public void Generate()
        {
            Opened = false;
        }

        public void Dispose()
        {
            Opened = false;
        }
    }
}