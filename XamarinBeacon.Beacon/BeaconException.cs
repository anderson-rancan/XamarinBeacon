using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinBeacon.Beacon
{
    public class BeaconException : Exception
    {
        public BeaconException() { }
        public BeaconException(string message) : base(message) { }
        public BeaconException(string message, Exception inner) : base(message, inner) { }
    }
}
