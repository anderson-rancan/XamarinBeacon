using System;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using static System.Console;

namespace XamarinBeacon.Console
{
    public static class Program
    {
        private readonly static BeaconManager _beaconManager = new BeaconManager();
        private readonly static BluetoothLEAdvertisementWatcher _watcher = new BluetoothLEAdvertisementWatcher { ScanningMode = BluetoothLEScanningMode.Active };

        public static void Main(string[] args)
        {
            _watcher.Received += WatcherOnReceived;
            _watcher.Stopped += WatcherOnStopped;
            _watcher.Start();

            var timer = new System.Threading.Timer(Refresh, null, 1000, 1000);

            ReadKey();

            timer.Dispose();
        }

        private static void Refresh(object state)
        {
            Clear();

            foreach (var beacon in _beaconManager.BluetoothBeacons)
            {
                WriteLine(beacon.BluetoothAddressAsString);
                WriteLine("{0} (beacon type: {1})", KnownManufacturer.FromManufacturerId(beacon.ManufacturerId).Name, beacon.BeaconType);
                WriteLine("RSSI: {0}", beacon.Rssi);

                foreach (var frame in beacon.BeaconFrames)
                {
                    WriteLine("---- frame ----");

                    if (frame is UidEddystoneFrame)
                    {
                        var uidEddystoneFrame = frame as UidEddystoneFrame;

                        WriteLine("Eddystone UID Frame");
                        WriteLine("ID: {0:X} / {1:X}", uidEddystoneFrame.NamespaceIdAsNumber, uidEddystoneFrame.InstanceIdAsNumber);
                    }
                    else if (frame is UrlEddystoneFrame)
                    {
                        var urlEddystoneFrame = frame as UrlEddystoneFrame;

                        WriteLine("Eddystone URL Frame");
                        WriteLine("URL: {0}", urlEddystoneFrame.CompleteUrl);
                    }
                    else if (frame is TlmEddystoneFrame)
                    {
                        var tlmEddystoneFrame = frame as TlmEddystoneFrame;

                        WriteLine("Eddystone Telemetry Frame");
                        WriteLine("Temperature {0}°C", tlmEddystoneFrame.TemperatureInC);
                        WriteLine("Battery: {0}mV", tlmEddystoneFrame.BatteryInMilliV);
                    }
                    else if (frame is ProximityBeaconFrame)
                    {
                        var proximityBeaconFrame = frame as ProximityBeaconFrame;

                        WriteLine("Proximity beacon frame");
                        WriteLine("Major: {0} / Minor: {1}", proximityBeaconFrame.MajorAsString, proximityBeaconFrame.MinorAsString);
                        WriteLine("TxPower: " + proximityBeaconFrame.TxPower);
                        WriteLine("Uuid: " + proximityBeaconFrame.UuidAsString);
                        WriteLine("Payload: " + BitConverter.ToString(proximityBeaconFrame.Payload));
                    }
                    else if (frame is IBeaconFrame)
                    {
                        var iBeaconFrame = frame as IBeaconFrame;

                        WriteLine("iBeacon frame");
                        WriteLine("Complete local name: {0}", iBeaconFrame.CompleteLocalName);
                    }
                    else
                    {
                        WriteLine("Unknown frame - not parsed by the library, write your own derived beacon frame type!");
                        WriteLine("Payload: " + BitConverter.ToString(frame.Payload));
                    }
                }

                WriteLine();
            }
        }

        private static void WatcherOnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            _beaconManager.ReceivedAdvertisement(args);
        }

        private static void WatcherOnStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            string errorMsg = null;
            if (args != null)
            {
                switch (args.Error)
                {
                    case BluetoothError.Success:
                        errorMsg = "WatchingSuccessfullyStopped";
                        break;
                    case BluetoothError.RadioNotAvailable:
                        errorMsg = "ErrorNoRadioAvailable";
                        break;
                    case BluetoothError.ResourceInUse:
                        errorMsg = "ErrorResourceInUse";
                        break;
                    case BluetoothError.DeviceNotConnected:
                        errorMsg = "ErrorDeviceNotConnected";
                        break;
                    case BluetoothError.DisabledByPolicy:
                        errorMsg = "ErrorDisabledByPolicy";
                        break;
                    case BluetoothError.NotSupported:
                        errorMsg = "ErrorNotSupported";
                        break;
                }
            }

            WriteLine(errorMsg);
            WriteLine("Press any key to continue");
            ReadKey();
        }
    }
}
