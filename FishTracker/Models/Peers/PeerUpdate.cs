using FishTracker.Helpers;
using FishTracker.Models.API;

namespace FishTracker.Models.Peers
{
    public class PeerUpdate
    {
        public static void PeerUpdateToDB(GetPeersObject apiInput, string connStr, int dbId)
        {
            AnnounceInputParameters announceInputParameters = new AnnounceInputParameters(apiInput);
            Peer peer = new Peer(announceInputParameters);
            
        }

    }
}
