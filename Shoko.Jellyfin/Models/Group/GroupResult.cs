using System;
using System.Collections.Generic;

namespace Shoko.Jellyfin.Models.Group
{
    public class GroupResult
    {
        public List<object> Series { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<TitleReference> Titles { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset Added { get; set; }
        public DateTimeOffset Edited { get; set; }
        public long Year { get; set; }
        public DateTimeOffset Air { get; set; }
        public long Size { get; set; }
        public string Rating { get; set; }
        public List<RoleElement> Roles { get; set; }
        public List<string> Tags { get; set; }
        public Art Art { get; set; }
    }
}