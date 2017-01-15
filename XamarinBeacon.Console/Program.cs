using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalBeaconLibrary.Beacon;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using XamarinBeacon.Beacon;
using static System.Console;

namespace XamarinBeacon.Console
{
    public static class Program
    {
        private static DeviceWatcher _deviceWatcher;
        private readonly static BeaconManager _beaconManager = new BeaconManager();
        private readonly static BluetoothLEAdvertisementWatcher _watcher = new BluetoothLEAdvertisementWatcher { ScanningMode = BluetoothLEScanningMode.Active };

        public static void Main(string[] args)
        {
            _watcher.Received += WatcherOnReceived;
            _watcher.Stopped += WatcherOnStopped;
            _watcher.Start();

            _deviceWatcher = DeviceInformation.CreateWatcher();
            _deviceWatcher.Added += DeviceAdded;
            _deviceWatcher.Updated += DeviceUpdated;

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
                    WriteLine("---- {0} ----", frame.GetType().Name);

                    if (frame is UidEddystoneFrame)
                    {
                        var uidEddystoneFrame = frame as UidEddystoneFrame;

                        WriteLine("ID: {0:X} / {1:X}", uidEddystoneFrame.NamespaceIdAsNumber, uidEddystoneFrame.InstanceIdAsNumber);
                    }
                    else if (frame is UrlEddystoneFrame)
                    {
                        var urlEddystoneFrame = frame as UrlEddystoneFrame;

                        WriteLine("URL: {0}", urlEddystoneFrame.CompleteUrl);
                    }
                    else if (frame is TlmEddystoneFrame)
                    {
                        var tlmEddystoneFrame = frame as TlmEddystoneFrame;

                        WriteLine("Temperature {0}°C", tlmEddystoneFrame.TemperatureInC);
                        WriteLine("Battery: {0}mV", tlmEddystoneFrame.BatteryInMilliV);
                    }
                    else if (frame is ProximityBeaconFrame)
                    {
                        var proximityBeaconFrame = frame as ProximityBeaconFrame;

                        WriteLine("Major: {0} / Minor: {1}", proximityBeaconFrame.MajorAsString, proximityBeaconFrame.MinorAsString);
                        WriteLine("TxPower: " + proximityBeaconFrame.TxPower);
                        WriteLine("Uuid: " + proximityBeaconFrame.UuidAsString.ToUpperInvariant());
                    }
                    else if (frame is AxaBatTempHumFrame)
                    {
                        var axaBeaconFrame = frame as AxaBatTempHumFrame;

                        WriteLine("Battery: {0} %", axaBeaconFrame.Battery);
                        WriteLine("Temperature: {0} °C", axaBeaconFrame.Temperature);
                        WriteLine("Humidity: {0} %", axaBeaconFrame.Humidity);
                    }
                    else if (frame is AxaCompleteNameFrame)
                    {
                        var axaCompleteNameFrame = frame as AxaCompleteNameFrame;

                        WriteLine("Complete name: {0}", axaCompleteNameFrame.CompleteName);
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
            /*
            var address = string.Join("-", BitConverter.GetBytes(args.BluetoothAddress).Reverse().Select(b => b.ToString("X2"))).Substring(6);

            using (var writer = new System.IO.StreamWriter("C:\\Users\\aranc\\Documents\\" + address + ".txt", true))
            {
                writer.WriteLine(new string('-', 20));

                writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothAddress: {0}", args.BluetoothAddress);
                writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.Timestamp: {0}", args.Timestamp);
                writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.RawSignalStrengthInDBm", args.RawSignalStrengthInDBm);
                writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.AdvertisementType", args.AdvertisementType);
                writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement");

                writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.LocalName: {0}", args.Advertisement.LocalName);
                if (args.Advertisement.Flags.HasValue) writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.Flags: {0}", args.Advertisement.Flags.Value);

                for (int i = 0; i < args.Advertisement.DataSections.Count; i++)
                {
                    writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.DataSections[{0}].DataType: {1}", i, args.Advertisement.DataSections[i].DataType);
                    writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.DataSections[{0}].Data: {1}", i, string.Join("-", args.Advertisement.DataSections[i].Data.ToArray()));
                }

                for (int i = 0; i < args.Advertisement.ManufacturerData.Count; i++)
                {
                    writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.ManufacturerData[{0}].CompanyId: {1}", i, args.Advertisement.ManufacturerData[i].CompanyId);
                    writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.ManufacturerData[{0}].DataType: {1}", i, string.Join("-", args.Advertisement.ManufacturerData[i].Data.ToArray()));
                }

                for (int i = 0; i < args.Advertisement.ServiceUuids.Count; i++)
                {
                    writer.WriteLine("BluetoothLEAdvertisementReceivedEventArgs.BluetoothLEAdvertisement.ServiceUuids[{0}]: {1}", i, args.Advertisement.ServiceUuids[i].ToString());
                }

                writer.WriteLine(new string('-', 20));
            }
            */

            _beaconManager.ReceivedAdvertisement(args);
        }

        private static async void DeviceAdded(DeviceWatcher watcher, DeviceInformation device)
        {
            try
            {
                var service = await GattDeviceService.FromIdAsync(device.Id);
                WriteLine("Opened Service!!");
            }
            catch
            {
                WriteLine("Failed to open service.");
            }
        }

        private static void DeviceUpdated(DeviceWatcher watcher, DeviceInformationUpdate update)
        {
            WriteLine($"Device updated: {update.Id}");
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
