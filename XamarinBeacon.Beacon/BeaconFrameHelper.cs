﻿using System;

namespace XamarinBeacon.Beacon
{
    public static class BeaconFrameHelper
    {
        /// <summary>
        /// Number of bytes of the Eddystone header that is the same in all frame types.
        /// * 2 bytes for 0xAA, 0xFE to identify Eddystone.
        /// * 1 byte for the frame type.
        /// </summary>
        public const int EddystoneHeaderSize = 3;

        /// <summary>
        /// UUID of BluetoothGattCharacteristic,used to read data of temperature and humidity
        /// </summary>
        public readonly static Guid UUID_READTH = Guid.Parse("0000fff3-0000-1000-8000-00805f9b34fb");

        /// <summary>
        /// Characteristic value of temperature and humidity ,Used to enable notification
        /// </summary>
        public readonly static Guid UUID_ENABLETH = Guid.Parse("0000fff4-0000-1000-8000-00805f9b34fb");

        public enum EddystoneFrameType : byte
        {
            UidFrameType = 0x00,
            UrlFrameType = 0x10,
            TelemetryFrameType = 0x20
        }

        /// <summary>
        /// Analyzes the payload of the Bluetooth Beacon frame and instantiates
        /// the according specialized Bluetooth frame class.
        /// Currently handles Eddystone frames.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns>Base class for Bluetooth frames, which is either a specialized
        /// class or an UnknownBeaconFrame.</returns>
        public static BeaconFrameBase CreateEddystoneBeaconFrame(this byte[] payload)
        {
            if (!payload.IsEddystoneFrameType()) return null;
            switch (payload.GetEddystoneFrameType())
            {
                case EddystoneFrameType.UidFrameType:
                    return new UidEddystoneFrame(payload);
                case EddystoneFrameType.UrlFrameType:
                    return new UrlEddystoneFrame(payload);
                case EddystoneFrameType.TelemetryFrameType:
                    return new TlmEddystoneFrame(payload);
                case null:
                    return null;
            default:
                    return new UnknownBeaconFrame(payload);
        }
    }

        /// <summary>
        /// Analyzes the header of the payload to check if the frame is
        /// a valid Eddystone frame.
        /// It needs to start with 0xAA 0xFE and then as the third byte
        /// have the Eddystone frame type according to the specification.
        /// 
        /// Does not analyze if the rest of the contents are valid
        /// according to the specification - this task is up to the
        /// spezialized handler classes.
        /// </summary>
        /// <param name="payload">Frame payload to analyze.</param>
        /// <returns>True if this is an Eddystone frame, false if not.</returns>
        public static bool IsEddystoneFrameType(this byte[] payload)
        {
            if (payload == null || payload.Length < 3) return false;

            if (!(payload[0] == 0xAA && payload[1] == 0xFE)) return false;

            var frameTypeByte = payload[2];
            return Enum.IsDefined(typeof(EddystoneFrameType), frameTypeByte);
        }

        /// <summary>
        /// Retrieve the Eddystone frame type for this frame.
        /// </summary>
        /// <param name="payload">Frame payload to analyze.</param>
        /// <returns>The Eddystone frame type that has been determined.
        /// If it is not a valid or known Eddystone frame type, returns null.</returns>
        public static EddystoneFrameType? GetEddystoneFrameType(this byte[] payload)
        {
            if (!IsEddystoneFrameType(payload)) return null;
            return (EddystoneFrameType)payload[2];
        }

        /// <summary>
        /// Create the first three bytes of the Eddystone payload.
        /// 0xAA 0xFE [Eddystone type].
        /// </summary>
        /// <param name="eddystoneType">Eddystone type to use for this frame.</param>
        /// <returns>Byte array with the size of 3 containing the Eddystone frame header.</returns>
        public static byte[] CreateEddystoneHeader(EddystoneFrameType eddystoneType)
        {
            return new byte[] { 0xAA, 0xFE, (byte)eddystoneType };
        }

        public static bool IsProximityBeaconPayload(ushort companyId, byte[] manufacturerData)
        {
            return companyId == ProximityBeaconFrame.CompanyId &&
                   manufacturerData.Length >= 23 &&
                   manufacturerData[0] == ProximityBeaconFrame.DataType &&
                   manufacturerData[1] == ProximityBeaconFrame.DataLength;
        }
    }
}
