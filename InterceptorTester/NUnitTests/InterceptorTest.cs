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
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace ConsoleApplication1
{
    [TestFixture()]
    public class InterceptorTest
    {
        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }

		[Test()]
		public KeyValuePair<JObject, string> createInterceptor()
		{
			//TODO: Set this up
			Interceptor newInt = new Interceptor(TestGlobals.testServer, TestGlobals.validSerial);
			Test mTest = new Test(newInt);
			HttpClient client = new HttpClient();
			//TODO: Initialize the client properly - add session token to header, etc.
			//client.setup;
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}

		[Test()]
		public KeyValuePair<JObject, string> getSingleInterceptor()
		{
			string query = "/API/Interceptor/" + TestGlobals.validSerial;
			GenericRequest getInt = new GenericRequest(TestGlobals.testServer, query, null);
			Test mTest = new Test(getInt);
			HttpClient client = new HttpClient();
			//TODO: Initialize the client properly - add session token to header, etc.
			//client.setup;
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}

		[Test()]
		public KeyValuePair<JObject, string> getMultipleInterceptors()
		{
			string query = "API/Interceptor/?LocId=" + TestGlobals.validLocId;
			GenericRequest getInt = new GenericRequest (TestGlobals.testServer, query, null);
			Test mTest = new Test (getInt);
			HttpClient client = new HttpClient ();
			AsyncContext.Run (async() => await new HTTPSCalls ().runTest (mTest, HTTPOperation.GET, client));
			Assert.AreEqual ("201", HTTPCalls.result.Value);
			return HTTPCalls.result;
		}
	}

}
