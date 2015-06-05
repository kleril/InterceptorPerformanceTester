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
	public class DeviceStatusTest
    {
        static StreamWriter results;

		DeviceStatusJSON status;

		[TestFixtureSetUp]
		public void setup()
		{
			status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = "";
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "asdf";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.dynCodeFormat = err;
			status.errorLog = err;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

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
		public void ValidSerial()
		{
			DeviceStatusJSON status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = "";
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "asdf";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.dynCodeFormat = err;
			status.errorLog = err;
            status.intSerial = TestGlobals.validSerial;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

			DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("ValidSerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(statusTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}


		[Test()]
		public void InvalidSerial()
		{
			DeviceStatusJSON status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = null;
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "asdf";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.errorLog = err;
			status.intSerial = TestGlobals.invalidSerial;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

			DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("BadSerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(statusTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		public void EmptySerial()
		{
			DeviceStatusJSON status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = "";
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "asdf";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.errorLog = err;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

			DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("EmptySerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(statusTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		public void NullSerial()
		{
			DeviceStatusJSON status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = null;
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "asdf";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.errorLog = err;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

			DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("NullSerial");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(statusTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		public void AlertDataStore()
		{
			DeviceStatusJSON status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = null;
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "<timestamp>///900///bypassmode";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.errorLog = err;
			status.intSerial = TestGlobals.validSerial;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

			DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("AlertDataStore");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(statusTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);

		}
		
	}
}

