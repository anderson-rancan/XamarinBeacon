using System;
using System.Collections.ObjectModel;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace XamarinBeacon.Beacon
{
    public class BeaconManager
    {
        private readonly BluetoothLEAdvertisementWatcher _bluetoothWatcher;

        public ObservableCollection<Beacon> BluetoothBeacons { get; set; } = new ObservableCollection<Beacon>();

        public BeaconManager()
        {
            _bluetoothWatcher = new BluetoothLEAdvertisementWatcher { ScanningMode = BluetoothLEScanningMode.Active };
            _bluetoothWatcher.Received += WatcherOnReceived;
            _bluetoothWatcher.Stopped += WatcherOnStopped;
        }

        public void StartWatching()
        {
            _bluetoothWatcher.Start();
        }

        private void WatcherOnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            try
            {
                ReceivedAdvertisement(eventArgs);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }

        private void WatcherOnStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
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

            Console.WriteLine(errorMsg);
        }

        private void ReceivedAdvertisement(BluetoothLEAdvertisementReceivedEventArgs btAdv)
        {
            if (btAdv == null)
                return;

            foreach (var bluetoothBeacon in BluetoothBeacons)
            {
                if (bluetoothBeacon.BluetoothAddress == btAdv.BluetoothAddress)
                {
                    Console.WriteLine("Updating existing beacon");
                    bluetoothBeacon.UpdateBeacon(btAdv);
                    return;
                }
            }

            Console.WriteLine("Received advertisement from new Beacon");
            BluetoothBeacons.Add(new Beacon(btAdv));
        }
    }
}
