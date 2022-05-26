using BencodeNET.Objects;
using FishTracker.Models.Peers;
using System.Net;

namespace FishTracker.Models
{
    public class Peer
    {
        /// <summary>
        /// Client IP EndPoint Information.
        /// </summary>
        public IPEndPoint ClientAddress { get; private set; }

        /// <summary>
        /// Random id of the client.
        /// </summary>
        public string PeerId { get; private set; }

        /// <summary>
        /// Unique identifier of the client.
        /// </summary>
        public string UniqueId { get; private set; }

        /// <summary>
        /// The amount of data downloaded by the client during the session. (In bytes)
        /// </summary>
        public long DownLoaded { get; private set; }

        /// <summary>
        /// The amount of data uploaded by the client during the session. (In bytes)
        /// </summary>
        public long Uploaded { get; private set; }

        /// <summary>
        /// Client download speed. (Unit: Byte/ second)
        /// </summary>
        public long DownloadSpeed { get; private set; }

        /// <summary>
        /// Client upload speed. (Unit: Byte/ second)
        /// </summary>
        public long UploadSpeed { get; private set; }

        /// <summary>
        /// Whether the client has completed the current seed, True for completed, False for not completed.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// The last time the Tracker was requested.
        /// </summary>
        public DateTime LastRequestTrackerTime { get; private set; }

        /// <summary>
        /// The number of downloads required by Peer.
        /// </summary>
        public long Left { get; private set; }

        public Peer() { }

        public Peer(AnnounceInputParameters inputParameters)
        {
            UniqueId = inputParameters.PeerId;

            UpdateStatus(inputParameters);
        }

        /// <summary>
        /// Update the status of the Peer based on the input parameters.
        /// </summary>
        /// <param name="inputParameters">Parameters passed by the BT client when it requests the Tracker server.</param>
        public void UpdateStatus(AnnounceInputParameters inputParameters)
        {
            var now = DateTime.Now;

            var elapsedTime = (now - LastRequestTrackerTime).TotalSeconds;
            if (elapsedTime < 1) elapsedTime = 1;

            ClientAddress = inputParameters.ClientAddress;

            DownloadSpeed = (int)((inputParameters.Downloaded - DownLoaded) / elapsedTime);
            DownLoaded = inputParameters.Downloaded;
            UploadSpeed = (int)((inputParameters.Uploaded) / elapsedTime);
            Uploaded = inputParameters.Uploaded;
            Left = inputParameters.Left;
            PeerId = inputParameters.PeerId;
            LastRequestTrackerTime = now;

            //  If there is no remaining data, the Peer download has been finished.
            if (Left == 0) IsCompleted = true;
        }


        /// <summary>
        /// The Peer information is encoded into the B dictionary.
        /// </summary>
        public BDictionary ToEncodedDictionary()
        {
            return new BDictionary
            {
                {TrackerServerConsts.PeerIdKey,new BString(PeerId)},
                {TrackerServerConsts.Ip,new BString(ClientAddress.Address.ToString())},
                {TrackerServerConsts.Port,new BNumber(ClientAddress.Port)}
            };
        }

        /// <summary>
        /// To compactly encoded the Peer information into byte groups.
        /// </summary>
        public byte[] ToBytes()
        {
            var portBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ClientAddress.Port));
            var addressBytes = ClientAddress.Address.GetAddressBytes();

            var resultBytes = new byte[portBytes.Length + addressBytes.Length];

            // the 4 bytes in the header are the IP address, and the 2 bytes in the tail are the port information
            Array.Copy(addressBytes, resultBytes, addressBytes.Length);
            Array.Copy(portBytes, 0, resultBytes, addressBytes.Length, portBytes.Length);

            return resultBytes;
        }
    }
}
