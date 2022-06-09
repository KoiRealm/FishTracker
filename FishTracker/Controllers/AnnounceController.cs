using BencodeNET.Objects;
using FishTracker.Manager;
using FishTracker.Models;
using FishTracker.Models.API;
using FishTracker.Models.Peers;
using Microsoft.AspNetCore.Mvc;

namespace FishTracker.Controllers
{
    [Route("api/announce")]
    [ApiController]
    public class AnnounceController : ControllerBase
    {
        private readonly IBitTorrentManager? _bitTorrentManager;


        public AnnounceController( IBitTorrentManager bitTorrentManager)
        {
            _bitTorrentManager = bitTorrentManager;
        }

        [HttpGet("test")]
        public FileContentResult GetPeersInfo([FromQuery]GetPeersObject getPeersObject)
        {
            // Get IP address from Context If client doesn't tell theirs.
            if (string.IsNullOrEmpty(getPeersObject.Ip)) getPeersObject.Ip = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            AnnounceInputParameters inputPara = getPeersObject;
            var resultDict = new BDictionary();

            // According to BitTorrent Protocol, detail message will be returned directly if error occurs.
            if (inputPara.Error.Count == 0)
            {
                _bitTorrentManager.UpdatePeer(getPeersObject.Info_Hash, inputPara);
                _bitTorrentManager.ClearZombiePeers(getPeersObject.Info_Hash, TimeSpan.FromMinutes(10));
                var peers = _bitTorrentManager.GetPeers(getPeersObject.Info_Hash);

                HandlePeersData(resultDict, peers, inputPara);
                // Client's waiting interval.
                resultDict.Add(TrackerServerConsts.IntervalKey, new BNumber((int)TimeSpan.FromSeconds(30).TotalSeconds));
                // Client's minimal waiting interval.
                resultDict.Add(TrackerServerConsts.MinIntervalKey, new BNumber((int)TimeSpan.FromSeconds(30).TotalSeconds));
                // Tracker Server's identity.
                resultDict.Add(TrackerServerConsts.TrackerIdKey, new BString("FishTracker"));
                // Completed peers count.
                resultDict.Add(TrackerServerConsts.CompleteKey, new BNumber(_bitTorrentManager.GetComplete(getPeersObject.Info_Hash)));
                // Incompleted peers count.
                resultDict.Add(TrackerServerConsts.IncompleteKey, new BNumber(_bitTorrentManager.GetInComplete(getPeersObject.Info_Hash)));
            }
            else
            {
                resultDict = inputPara.Error;
            }

            // Return "Content-Type:text/plain" by using File()
            return File(resultDict.EncodeAsBytes(),"text/plain");
        }

        /// <summary>
        /// Transform peers collection into result BDictionary.
        /// </summary>
        private void HandlePeersData(BDictionary resultDict, IReadOnlyList<Peer> peers, AnnounceInputParameters inputParameters)
        {

            var total = Math.Min(peers.Count, inputParameters.PeerWantCount);

            // Check if BT client need to enable compact mode.
            if (inputParameters.IsEnableCompact) 
            {
                var compactResponse = new byte[total * 6];
                for (int index = 0; index < total; index++)
                {
                    var peer = peers[index];
                    Buffer.BlockCopy(peer.ToBytes(), 0, compactResponse, (total - 1) * 6, 6);
                }

                resultDict.Add(TrackerServerConsts.PeersKey, new BString(compactResponse));
            }
            else
            {
                var nonCompactResponse = new BList();
                for (int index = 0; index < total; index++)
                {
                    var peer = peers[index];
                    nonCompactResponse.Add(peer.ToEncodedDictionary());
                }

                resultDict.Add(TrackerServerConsts.PeersKey, nonCompactResponse);
            }
        }
    }
}
