using System;
using System.Collections.Generic;
using Wox.WebApp.DomainModel;

namespace Wox.WebApp.Core.Service
{
    public interface IWebAppService : IDisposable
    {
        /// <summary>
        /// Initialize the instance of the service
        /// </summary>
        void Init();

        /// <summary>
        /// Search a webapp given terms
        /// </summary>
        /// <param name="terms">The terms to search</param>
        /// <returns>An enumerable of WebAppItem</returns>
        IEnumerable<WebAppItem> Search(IEnumerable<string> terms);

        /// <summary>
        /// Add a new WebAppItem using an url and keywords
        /// </summary>
        /// <param name="url">The url of the web app</param>
        /// <param name="keywords">The keywords used for search</param>
        void AddWebAppItem(string url, string keywords);

        /// <summary>
        /// Update the application to use to browse the webapp
        /// </summary>
        /// <param name="launcher">The executable to use</param>
        /// <param name="argumentPattern">The command line arguments to pass (used a template)</param>
        void UpdateLauncher(string launcher, string argumentPattern);

        /// <summary>
        /// Start a web app given it's url
        /// </summary>
        /// <param name="url">The url of the web app to start</param>
        void StartUrl(string url);

        /// <summary>
        /// Get the current configuration
        /// </summary>
        /// <returns>A WebAppConfigration instance</returns>
        WebAppConfiguration GetConfiguration();

        /// <summary>
        /// Remove an existing web app given it's url.
        /// </summary>
        /// <param name="url">The url of the web app to remove</param>
        void RemoveUrl(string url);

        /// <summary>
        /// Export the current configration to a file
        /// </summary>
        void Export();

        /// <summary>
        /// Returns true if a file exists
        /// </summary>
        /// <param name="path">The file name</param>
        /// <returns></returns>
        bool FileExists(string path);

        /// <summary>
        /// Import a configuration previously exported
        /// </summary>
        /// <param name="path">The file name of the configuration file to export</param>
        void Import(string path);

        /// <summary>
        /// Get information on an url, or null if no URL match the criteria
        /// </summary>
        /// <param name="url">The url</param>
        /// <returns>The informations about the url</returns>
        WebAppItem GetUrlInfo(string url);

        /// <summary>
        /// Edit informations about a web app.
        /// </summary>
        /// <param name="url">The url of the current webapp item</param>
        /// <param name="newUrl">The new url for the webapp item</param>
        /// <param name="newKeywords">The new keywords to use</param>
        void EditWebAppItem(string url, string newUrl, string newKeywords);
    }
}