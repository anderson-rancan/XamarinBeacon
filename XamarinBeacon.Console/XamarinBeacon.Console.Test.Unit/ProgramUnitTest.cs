using NUnit.Framework;

namespace XamarinBeacon.Console.Test.Unit
{
    [TestFixture]
    public class ProgramUnitTest
    {
        [Test]
        public void Main_ShouldNotThrowExceptions()
        {
            Assert.DoesNotThrow(() => Program.Main(null));
        }
    }
}
