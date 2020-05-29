using System;
using System.Globalization;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.IO;
using Microsoft.Extensions.Logging;
using Shoko.Jellyfin.Models;

namespace Shoko.Jellyfin
{
    public class ShokoItemResolver : IItemResolver
    {
        private static readonly CultureInfo UsCulture = new CultureInfo("en-US");

        /// <summary>
        ///     The _config
        /// </summary>
        private readonly IServerConfigurationManager _config;

        private readonly IFileSystem _fs;

        private readonly ILibraryManager _libraryManager;
        private readonly ILocalizationManager _localization;
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShokoItemResolver" /> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ShokoItemResolver(IServerConfigurationManager config, ILibraryManager libraryManager,
            ILocalizationManager localization, ILogger logger, IFileSystem fs)
        {
            _config = config;
            _libraryManager = libraryManager;
            _localization = localization;
            _logger = logger;
            _fs = fs;
        }

        public BaseItem ResolvePath(ItemResolveArgs args)
        {
            if (args.IsDirectory) return null;

            _logger.Log(LogLevel.Information, "Resolving for path: {0}", args.FileInfo.Name);

            //"api/ep/getbyfilename?filename=%s"
            var ep = Plugin.Instance.GetHttp<Episode>(
                $"ep/getbyfilename?filename={Uri.EscapeDataString(args.FileInfo.Name)}");


            return new Video {
                Name = ep.name,
                Path = args.Path
            };
        }

        public ResolverPriority Priority => ResolverPriority.First;
    }
}