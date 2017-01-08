using XamarinBeacon.Beacon;

namespace XamarinBeacon.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var worker = new BeaconManager();
            worker.StartWatching();

            System.Console.WriteLine("Application started");
            System.Console.ReadKey();
        }
    }
}
