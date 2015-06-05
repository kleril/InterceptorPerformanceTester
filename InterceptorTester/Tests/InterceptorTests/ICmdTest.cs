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
	public class ICmdTest
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
		public void ValidSerial()
		{
			ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
			Test validTest = new Test(validICmd);
			validTest.setTestName("ValidSerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(validTest, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());
            
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("200", statusCode);
		}

		[Test()]
		public void InvalidSerial()
		{
			ICmd invalidICmd = new ICmd(TestGlobals.testServer, TestGlobals.invalidSerial);
			Test invalidTest = new Test(invalidICmd);
			invalidTest.setTestName("BadSerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(invalidTest, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());
            
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);

		}

		[Test()]
		public void MissingSerial()
		{
			ICmd missingICmd = new ICmd(TestGlobals.testServer, null);
			Test missingTest = new Test(missingICmd);
			missingTest.setTestName("EmptySerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(missingTest, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());

            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
		}

		[Test()]
		public void NoQuery()
		{
			ICmd missingICmd = new ICmd(TestGlobals.testServer, null);
			missingICmd.noQuery = true;
			Test missingTest = new Test(missingICmd);
			missingTest.setTestName("NoQuery");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(missingTest, HTTPOperation.GET));
            results.WriteLine(HTTPSCalls.result.ToString());

            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("404", statusCode);
		}
	}
}

