using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Logging;

namespace Shoko.Emby
{
    public class ShokoItemResolver : IItemResolver
    {
        /// <summary>
        /// The _config
        /// </summary>
        private readonly IServerConfigurationManager _config;

        private readonly ILibraryManager _libraryManager;
        private static readonly CultureInfo UsCulture = new CultureInfo("en-US");
        private readonly ILocalizationManager _localization;
        private readonly ILogger _logger;
        private readonly IFileSystem _fs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShokoItemResolver"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public ShokoItemResolver(IServerConfigurationManager config, ILibraryManager libraryManager, ILocalizationManager localization, ILogger logger, IFileSystem fs)
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

            _logger.Log(LogSeverity.Info, "Resolving for path: {0}", args.FileInfo.Name);

            //"api/ep/getbyfilename?filename=%s"
            var ep = Plugin.Instance.GetHttp<Models.Episode>($"ep/getbyfilename?filename={Uri.EscapeDataString(args.FileInfo.Name)}");


            return new Video() {
                Name = ep.name,
                Path = args.Path,
            };

        }

        public ResolverPriority Priority { get => ResolverPriority.First; }
    }
}
