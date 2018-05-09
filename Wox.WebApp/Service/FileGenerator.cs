using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Service
{
    public class FileGenerator : IFileGenerator, IDisposable
    {
        private StreamWriter Writer { get; set; }
        private string Path { get; set; }

        public FileGenerator(string path)
        {
            Path = path;
            Writer = new StreamWriter(path);
        }

        public IFileGenerator AddLine(string line)
        {
            if (Writer != null)
            {
                Writer.WriteLine(line);
            }
            return this;
        }

        public void Generate()
        {
            if (Writer != null)
            {
                Writer.Close();
                Writer = null;
            }
        }

        public void Dispose()
        {
            if (Writer != null)
            {
                Writer.Close();
                Writer = null;
            }
        }
    }
}