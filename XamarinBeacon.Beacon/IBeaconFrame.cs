using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalBeaconLibrary.Beacon;

namespace UniversalBeaconLibrary.Beacon
{
    public class IBeaconFrame : BeaconFrameBase
    {

        private string _completeLocalName;
        public string CompleteLocalName
        {
            get { return _completeLocalName; }
            set
            {
                if (_completeLocalName == value) return;
                _completeLocalName = value;
                OnPropertyChanged();
            }
        }

        public IBeaconFrame(byte[] payload) : base(payload)
        {
            ParsePayload();
        }

        public void ParsePayload()
        {
            CompleteLocalName = new StreamReader(new MemoryStream(Payload)).ReadToEnd();


            //if (!IsValid()) return;

            //var newRangingData = (sbyte)Payload[BeaconFrameHelper.EddystoneHeaderSize];
            //if (newRangingData != RangingData)
            //{
            //    _rangingData = newRangingData;
            //    OnPropertyChanged(nameof(RangingData));
            //}

            //// Namespace ID
            //var newNamespaceId = new byte[10];
            //Array.Copy(Payload, BeaconFrameHelper.EddystoneHeaderSize + 1, newNamespaceId, 0, 10);
            //if (NamespaceId == null || !newNamespaceId.SequenceEqual(NamespaceId))
            //{
            //    _namespaceId = newNamespaceId;
            //    OnPropertyChanged(nameof(NamespaceId));
            //    OnPropertyChanged(nameof(NamespaceIdAsNumber));
            //}

            //// Instance ID
            //var newInstanceId = new byte[6];
            //Array.Copy(Payload, BeaconFrameHelper.EddystoneHeaderSize + 11, newInstanceId, 0, 6);
            //if (InstanceId == null || !newInstanceId.SequenceEqual(InstanceId))
            //{
            //    _instanceId = newInstanceId;
            //    OnPropertyChanged(nameof(InstanceId));
            //    OnPropertyChanged(nameof(InstanceIdAsNumber));
            //}
            //_instanceId = newInstanceId;
        }

        public override void Update(BeaconFrameBase otherFrame)
        {
            base.Update(otherFrame);
            ParsePayload();
        }
    }
}
