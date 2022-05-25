namespace FishTracker.Models.API
{
    public class GetPeersObject
    {
        /// <summary>
        /// Torrent's unique hash.
        /// </summary>
        public string? Info_Hash { get; set; }

        /// <summary>
        /// Client's IP Address.
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// Client's randomized Peer Id.
        /// </summary>
        public string? Peer_Id { get; set; }

        /// <summary>
        /// Client listening port number.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Uploaded file size.
        /// </summary>
        public long Uploaded { get; set; }

        /// <summary>
        /// Downloaded file size.
        /// </summary>
        public long Downloaded { get; set; }

        /// <summary>
        /// Calling event, see <see cref="TorrentEvent"/> for more information.
        /// </summary>
        public string? Event { get; set; }

        /// <summary>
        /// The size of the file that client left.
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
