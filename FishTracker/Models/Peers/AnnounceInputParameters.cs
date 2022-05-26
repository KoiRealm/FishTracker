using BencodeNET.Objects;
using FishTracker.Models.API;
using FishTracker.Models.Peers;
using System.Net;
using System.Web;

namespace FishTracker.Models
{
    public class AnnounceInputParameters
    {
        /// <summary>
        /// Client IP endpoint information.
        /// </summary>
        public IPEndPoint ClientAddress { get; }

        /// <summary>
        /// The unique Hash identifier of the seed.
        /// </summary>
        public string InfoHash { get; }

        /// <summary>
        /// Random Id of the client.
        /// </summary>
        public string PeerId { get; }

        /// <summary>
        /// Size of data that has been uploaded.
        /// </summary>
        public long Uploaded { get; }

        /// <summary>
        /// Size of data that has been downloaded.
        /// </summary>
        public long Downloaded { get; }

        /// <summary>
        /// Event representation, which can be converted to a specific value of the TorrentEvent enumeration.
        /// </summary>
        public TorrentEvent Event { get; }

        /// <summary>
        /// Remaining data to be downloaded on the client.
        /// </summary>
        public long Left { get; }

        /// <summary>
        /// Peer Indicates whether compression is allowed.
        /// </summary>
        public bool IsEnableCompact { get; }

        /// <summary>
        /// Peer Indicates the number of available peers that you want to obtain.
        /// </summary>
        public int PeerWantCount { get; }

        /// <summary>
        /// This dictionary contains exception information if an exception occurs during a request.
        /// </summary>
        public BDictionary Error { get; }


        public AnnounceInputParameters(GetPeersObject apiInput)
        {
            Error = new BDictionary();

            ClientAddress = ConvertClientAddress(apiInput);
            InfoHash = ConvertInfoHash(apiInput);
            Event = ConvertTorrentEvent(apiInput);
            PeerId = apiInput.Peer_Id;
            Uploaded = apiInput.Uploaded;
            Downloaded = apiInput.Downloaded;
            Left = apiInput.Left;
            IsEnableCompact = apiInput.Compact == 1;
            PeerWantCount = apiInput.NumWant ?? 30;
        }


        /// <summary>
        /// An implicit conversion definition of GetPeersInfoInput to the current type.
        /// </summary>
        public static implicit operator AnnounceInputParameters(GetPeersObject input)
        {
            return new AnnounceInputParameters(input);
        }



        /// <summary>
        /// Convert the IP address and port passed by the client to the IPEndPoint type.
        /// </summary>
        private IPEndPoint ConvertClientAddress(GetPeersObject apiInput)
        {
            if (IPAddress.TryParse(apiInput.Ip, out IPAddress ipAddress))
            {
                return new IPEndPoint(ipAddress, apiInput.Port);
            }

            return null;
        }

        /// <summary>
        /// Converts the client-passed string Event to a TorrentEvent enumeration.
        /// </summary>
        private TorrentEvent ConvertTorrentEvent(GetPeersObject apiInput)
        {
            switch (apiInput.Event)
            {
                case "started":
                    return TorrentEvent.Started;
                case "stopped":
                    return TorrentEvent.Stopped;
                case "completed":
                    return TorrentEvent.Completed;
                default:
                    return TorrentEvent.None;
            }
        }

        /// <summary>
        /// Converts the Info_hash parameter from a URL encoding to a standard string.
        /// </summary>
        private string ConvertInfoHash(GetPeersObject apiInput)
        {
            var infoHashBytes = HttpUtility.UrlDecodeToBytes(apiInput.Info_Hash);
            if (infoHashBytes == null)
            {
                Error.Add(TrackerServerConsts.FailureKey, new BString("info_hash 参数不能为空."));
                return null;
            }

            if (infoHashBytes.Length != 20)
            {
                Error.Add(TrackerServerConsts.FailureKey, new BString($"info_hash 参数的长度 {{{infoHashBytes.Length}}} 不符合 BT 协议规范."));
            }

            return BitConverter.ToString(infoHashBytes);
        }
    }
}
