using System;

namespace XamarinBeacon.Beacon
{
    public enum BeaconType
    {
        /// <summary>
        /// Bluetooth LE advertisment that is not recognized as one of the beacon formats supported by this library.
        /// </summary>
        Unknown,
        /// <summary>
        /// Beacon conforming to the Google Eddystone specification.
        /// </summary>
        Eddystone,
        /// <summary>
        /// Beacon conforming to the Apple iBeacon specification.
        /// </summary>
        iBeacon,

        Axaet
    }

    public static partial class Extension
    {
        public static readonly Guid EddystoneGuid = new Guid("0000FEAA-0000-1000-8000-00805F9B34FB");
        public static readonly Guid AxaetGuid = new Guid("0000fff6-0000-1000-8000-00805f9b34fb");

        public static Guid ToGuid(this BeaconType self)
        {
            switch (self)
            {
                case BeaconType.Axaet: return AxaetGuid;
                case BeaconType.Eddystone: return EddystoneGuid;
                case BeaconType.iBeacon:
                case BeaconType.Unknown:
                default: return Guid.Empty;
            }
        }

        public static BeaconType BeaconTypeFromGuid(Guid guid)
        {
            if (guid.Equals(AxaetGuid))
            {
                return BeaconType.Axaet;
            }
            else if (guid.Equals(EddystoneGuid))
            {
                return BeaconType.Eddystone;
            }
            else
            {
                return BeaconType.Unknown;
            }
        }
    }
}
