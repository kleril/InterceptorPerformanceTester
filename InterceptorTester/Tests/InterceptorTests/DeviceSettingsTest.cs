using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Configuration;
using Nito.AsyncEx;
using System.IO.Compression;
using ConsoleApplication1;

namespace InterceptorTester.Tests.InterceptorTests
{
	[TestFixture()]
	public class DeviceSettingsTest
    {

        static StreamWriter results;

        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();

            FileStream stream;
            stream = File.OpenWrite(TestGlobals.logFile);
            results = new StreamWriter(stream);
        }

        [TestFixtureTearDown()]
        public void tearDown()
        {
            results.Close();
        }
        [Test()]
		// Valid Serial
		public void ValidSerial() 
		{
			DeviceSetting dSetting1 = new DeviceSetting(TestGlobals.testServer, TestGlobals.validSerial);

			Test ValidSerial = new Test(dSetting1);
			ValidSerial.setTestName("ValidSerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(ValidSerial, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());

			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("200", statusCode);
		}

		[Test()]
		// Invalid Serial
		public void InvalidSerial() 
		{
			DeviceSetting dSetting2 = new DeviceSetting(TestGlobals.testServer, TestGlobals.invalidSerial);

			Test BadSerial = new Test(dSetting2);
			BadSerial.setTestName("BadSerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(BadSerial, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());

			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		// No Serial
		public void NoSerial() 
		{
			DeviceSetting dSetting3 = new DeviceSetting(TestGlobals.testServer, null);

			Test NoSerial = new Test(dSetting3);
			NoSerial.setTestName("NoSerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(NoSerial, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());

			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}
	}
}

