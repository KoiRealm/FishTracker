using BencodeNET.Objects;

namespace FishTracker.Models.Peers
{
    /// <summary>
    /// Tracker Server Consts
    /// </summary>
    public static class TrackerServerConsts
    {
        public static readonly BString PeerIdKey = new("peer id");
        public static readonly BString PeersKey = new("peers");
        public static readonly BString IntervalKey = new("interval");
        public static readonly BString MinIntervalKey = new("min interval");
        public static readonly BString TrackerIdKey = new("tracker id");
        public static readonly BString CompleteKey = new("complete");
        public static readonly BString IncompleteKey = new("incomplete");

        public static readonly BString Port = new("port");
        public static readonly BString Ip = new("ip");

        public static readonly string FailureKey = "failure reason";
    }
}
