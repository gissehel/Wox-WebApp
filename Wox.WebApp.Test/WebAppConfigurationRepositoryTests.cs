using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Wox.WebApp.Core.Service;
using Wox.WebApp.DomainModel;
using Wox.WebApp.Mock.Service;
using Wox.WebApp.Service;

namespace Wox.WebApp.Test
{
    public class WebAppConfigurationRepositoryTests
    {
        private ISystemWebAppService SystemWebAppService { get; set; }

        private IDataAccessService DataAccessService { get; set; }

        private IWebAppConfigurationRepository WebAppConfigurationRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            SystemWebAppService = new SystemWebAppServiceMock
            {
                ApplicationDataPath = Helper.GetTestPath(),
                ApplicationName = "TestDatabase",
            };
            DataAccessService = new DataAccessService(SystemWebAppService);
            WebAppConfigurationRepository = new WebAppConfigurationRepository(DataAccessService);
        }

        [TearDown]
        public void TearDown()
        {
            if (DataAccessService != null)
            {
                DataAccessService.Dispose();
            }

            WebAppConfigurationRepository = null;
            DataAccessService = null;
            SystemWebAppService = null;
        }

        public void Init()
        {
            DataAccessService.Init();
            WebAppConfigurationRepository.Init();
        }

        private class ResultStruct
        {
            public string Data { get; set; }
        }

        private IEnumerable<WebAppConfiguration> GetWebAppConfigurations() => DataAccessService
                .GetQuery("select profile, launcher, pattern from configuration;")
                .Returning<WebAppConfiguration>()
                .Reading("profile", (WebAppConfiguration configuration, string value) => configuration.Profile = value)
                .Reading("launcher", (WebAppConfiguration configuration, string value) => configuration.WebAppLauncher = value)
                .Reading("pattern", (WebAppConfiguration configuration, string value) => configuration.WebAppArgumentPattern = value)
                .Execute();

        private void EnsureSchema()
        {
            var schema = Helper.GetSchemaForTable(DataAccessService, "configuration");
            Assert.IsNotNull(schema);
            Assert.AreEqual("CREATE TABLE configuration (id integer primary key, profile text, launcher text, pattern text)", schema);
        }

        private void CreateOldSchema()
        {
            DataAccessService.GetQuery("create table if not exists configuration (id integer primary key, launcher text, pattern text);").Execute();
        }
        private void CreateNewSchema()
        {
            DataAccessService.GetQuery("create table if not exists configuration (id integer primary key, profile text, launcher text, pattern text);").Execute();
        }

        [Test]
        public void UpgradeFromScratch()
        {
            Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(0, configurations.Count());
        }

        [Test]
        public void UpgradeFromOldVersionWithoutData()
        {
            DataAccessService.Init();
            CreateOldSchema();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(0, configurations.Count());
        }

        [Test]
        public void UpgradeFromNewVersionWithoutData()
        {
            DataAccessService.Init();
            CreateNewSchema();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(0, configurations.Count());
        }

        [Test]
        public void UpgradeFromOldVersionWithData()
        {
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'launcher', 'args');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(1, configurations.Count());
            Assert.AreEqual("default", configurations.First().Profile);
            Assert.AreEqual("launcher", configurations.First().WebAppLauncher);
            Assert.AreEqual("args", configurations.First().WebAppArgumentPattern);
        }

        [Test]
        public void UpgradeFromNewVersionWithData()
        {
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'default', 'launcher', 'args');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(1, configurations.Count());
            Assert.AreEqual("default", configurations.First().Profile);
            Assert.AreEqual("launcher", configurations.First().WebAppLauncher);
            Assert.AreEqual("args", configurations.First().WebAppArgumentPattern);
        }

        [Test]
        public void UpgradeFromOldVersionWithManyData()
        {
            DataAccessService.Init();
            CreateOldSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'launcher', 'args');").Execute();
            DataAccessService.GetQuery("insert into configuration values (2, 'launcher2', 'args2');").Execute();
            DataAccessService.GetQuery("insert into configuration values (3, 'launcher3', 'args2');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(1, configurations.Count());
            Assert.AreEqual("default", configurations.First().Profile);
            Assert.AreEqual("launcher", configurations.First().WebAppLauncher);
            Assert.AreEqual("args", configurations.First().WebAppArgumentPattern);
        }

        [Test]
        public void UpgradeFromNewVersionWithManyData()
        {
            DataAccessService.Init();
            CreateNewSchema();
            DataAccessService.GetQuery("insert into configuration values (1, 'default', 'launcher', 'args');").Execute();
            DataAccessService.GetQuery("insert into configuration values (2, 'profile2', 'launcher2', 'args2');").Execute();
            DataAccessService.GetQuery("insert into configuration values (3, 'profile3', 'launcher3', 'args3');").Execute();
            WebAppConfigurationRepository.Init();
            EnsureSchema();
            var configurations = GetWebAppConfigurations();
            Assert.AreEqual(3, configurations.Count());
            Assert.AreEqual("default", configurations.First().Profile);
            Assert.AreEqual("launcher", configurations.First().WebAppLauncher);
            Assert.AreEqual("args", configurations.First().WebAppArgumentPattern);
            Assert.AreEqual("profile2", configurations.ElementAt(1).Profile);
            Assert.AreEqual("launcher2", configurations.ElementAt(1).WebAppLauncher);
            Assert.AreEqual("args2", configurations.ElementAt(1).WebAppArgumentPattern);
            Assert.AreEqual("profile3", configurations.ElementAt(2).Profile);
            Assert.AreEqual("launcher3", configurations.ElementAt(2).WebAppLauncher);
            Assert.AreEqual("args3", configurations.ElementAt(2).WebAppArgumentPattern);
        }
    }
}