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

		public static void createInterceptor()
		{
			//TODO: Set up JSON
			InterceptorJSON intjson = new InterceptorJSON(LocationTest.getLocId(), "Cozumo Office", "cozumo123", TestGlobals.validSerial);
			Interceptor newInt = new Interceptor(TestGlobals.testServer, TestGlobals.validSerial, intjson);

			Test mTest = new Test(newInt);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
			Console.WriteLine(HTTPSCalls.result.Value.ToString());
		}

		[Test()]
		public KeyValuePair<JObject, string> getSingleInterceptor()
		{
			string query = "/API/Interceptor/" + TestGlobals.validSerial;
			GenericRequest getInt = new GenericRequest(TestGlobals.testServer, query, null);
			Test mTest = new Test(getInt);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}

		[Test()]
		public KeyValuePair<JObject, string> getMultipleInterceptors()
		{
			string query = "/API/Interceptor/?LocId=" + TestGlobals.locIdCreated;
			GenericRequest getInt = new GenericRequest (TestGlobals.testServer, query, null);
			Test mTest = new Test (getInt);
			HttpClient client = new HttpClient ();
			AsyncContext.Run (async() => await new HTTPSCalls ().runTest (mTest, HTTPOperation.GET, client));
			Assert.AreEqual ("201", HTTPCalls.result.Value);
			return HTTPCalls.result;
		}
	}

}
