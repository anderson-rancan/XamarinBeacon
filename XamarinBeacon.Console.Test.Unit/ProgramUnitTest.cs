using NUnit.Framework;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace XamarinBeacon.Console.Test.Unit
{
    [TestFixture]
    public class ProgramUnitTest
    {
        [Test]
        public void Main_ShouldNotThrowExceptions()
        {
            Assert.DoesNotThrow(() => Task.Factory.StartNew(() => Program.Main(null)));
        }
    }
}
