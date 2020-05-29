using System.Collections.Generic;

namespace Shoko.Jellyfin.Models
{
    public class Art
    {
        public List<ArtReference> Banner { get; set; }
        public List<ArtReference> Fanart { get; set; }
        public List<ArtReference> Thumb { get; set; }
    }
}