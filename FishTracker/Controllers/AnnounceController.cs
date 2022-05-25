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
        public ContentResult GetPeersInfo([FromQuery]GetPeersObject getPeersObject)
        {
            // 如果 BT 客户端没有传递 IP，则通过 Context 获得。
            if (string.IsNullOrEmpty(getPeersObject.Ip)) getPeersObject.Ip = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            // 本机测试用。
            getPeersObject.Ip = "127.0.0.1";

            AnnounceInputParameters inputPara = getPeersObject;
            var resultDict = new BDictionary();

            // 如果产生了错误，则不执行其他操作，直接返回结果。
            if (inputPara.Error.Count == 0)
            {
                _bitTorrentManager.UpdatePeer(getPeersObject.Info_Hash, inputPara);
                _bitTorrentManager.ClearZombiePeers(getPeersObject.Info_Hash, TimeSpan.FromMinutes(10));
                var peers = _bitTorrentManager.GetPeers(getPeersObject.Info_Hash);

                HandlePeersData(resultDict, peers, inputPara);

                // 构建剩余字段信息
                // 客户端等待时间
                resultDict.Add(TrackerServerConsts.IntervalKey, new BNumber((int)TimeSpan.FromSeconds(30).TotalSeconds));
                // 最小等待间隔
                resultDict.Add(TrackerServerConsts.MinIntervalKey, new BNumber((int)TimeSpan.FromSeconds(30).TotalSeconds));
                // Tracker 服务器的 Id
                resultDict.Add(TrackerServerConsts.TrackerIdKey, new BString("Tracker-DEMO"));
                // 已完成的 Peer 数量
                resultDict.Add(TrackerServerConsts.CompleteKey, new BNumber(_bitTorrentManager.GetComplete(getPeersObject.Info_Hash)));
                // 非做种状态的 Peer 数量
                resultDict.Add(TrackerServerConsts.IncompleteKey, new BNumber(_bitTorrentManager.GetInComplete(getPeersObject.Info_Hash)));
            }
            else
            {
                resultDict = inputPara.Error;
            }

            // 写入响应结果。
            return Content(resultDict.EncodeAsString());
        }

        /// <summary>
        /// 将 Peer 集合的数据转换为 BT 协议规定的格式
        /// </summary>
        private void HandlePeersData(BDictionary resultDict, IReadOnlyList<Peer> peers, AnnounceInputParameters inputParameters)
        {
            var total = Math.Min(peers.Count, inputParameters.PeerWantCount);
            //var startIndex = new Random().Next(total);

            // 判断当前 BT 客户端是否需要紧凑模式的数据。
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
