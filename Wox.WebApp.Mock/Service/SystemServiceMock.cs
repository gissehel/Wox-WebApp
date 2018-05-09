﻿using System.Collections.Generic;
using Wox.WebApp.Core.Service;

namespace Wox.WebApp.Mock.Service
{
    public class SystemServiceMock : ISystemService
    {
        public string ApplicationDataPath { get; set; }

        public List<KeyValuePair<string, string>> CommandLineStarted { get; private set; } = new List<KeyValuePair<string, string>>();

        public void StartCommandLine(string command, string arguments)
        {
            CommandLineStarted.Add(new KeyValuePair<string, string>(command, arguments));
        }
    }
}