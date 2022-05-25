namespace FishTracker.Models.API
{
    public class GetPeersObject
    {
        /// <summary>
        /// Torrent's unique hash.
        /// </summary>
        public string? Info_Hash { get; set; }

        /// <summary>
        /// An optional parameter giving the IP (or dns name) which this peer is at.
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// A string of length 20 which this downloader uses as its id.
        /// </summary>
        public string? Peer_Id { get; set; }

        /// <summary>
        /// The port number this peer is listening on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The total amount uploaded so far, encoded in base ten ascii.
        /// </summary>
        public long Uploaded { get; set; }

        /// <summary>
        /// The total amount downloaded so far, encoded in base ten ascii.
        /// </summary>
        public long Downloaded { get; set; }

        /// <summary>
        /// Calling event, see <see cref="TorrentEvent"/> for more information.
        /// </summary>
        public string? Event { get; set; }

        /// <summary>
        /// The number of bytes this peer still has to download, encoded in base ten ascii.
        /// </summary>
        public long Left { get; set; }

        /// <summary>
        /// If client want to use Compact mode.
        /// </summary>
        public int Compact { get; set; }

        /// <summary>
        /// The Peer amount that client intend to get.
        /// </summary>
        public int? NumWant { get; set; }
    }
}
