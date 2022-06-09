using BencodeNET.Objects;

namespace FishTracker.Models.Peers
{
    /// <summary>
    /// The status about BitTorrent
    /// </summary>
    public class BitTorrentStatus
    {
        /// <summary>
        /// Downloaded Peers count.
        /// </summary>
        public BNumber Downloaded { get; set; }

        /// <summary>
        /// Completed Peers download count.
        /// </summary>
        public BNumber Completed { get; set; }

        /// <summary>
        /// Downloading Peers count.
        /// </summary>
        public BNumber InCompleted { get; set; }

        public BitTorrentStatus()
        {
            Downloaded = new BNumber(0);
            Completed = new BNumber(0);
            InCompleted = new BNumber(0);
        }
    }
}
