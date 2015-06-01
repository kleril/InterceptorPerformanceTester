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

        //Run deletes
        [TestFixtureTearDown()]
        public void teardown()
        {
            GenericRequest intReq = new GenericRequest (TestGlobals.testServer, "/api/interceptor/"+TestGlobals.intIdCreated, null);
            Console.WriteLine(intReq.getUri());
            GenericRequest locReq = new GenericRequest (TestGlobals.testServer, "/api/location/"+TestGlobals.locIdCreated, null);
            GenericRequest orgReq = new GenericRequest(TestGlobals.testServer, "/api/organization/" + TestGlobals.orgIdCreated, null);
            Test intTest = new Test(intReq);
            Test locTest = new Test(locReq);
            Test orgTest = new Test(orgReq);
            HttpClient client;
            /*client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(intTest, HTTPOperation.DELETE, client));
            Console.WriteLine(HTTPSCalls.result.Value);
            */
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(locTest, HTTPOperation.DELETE, client));
            Console.WriteLine(HTTPSCalls.result.Value);

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken(); 
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(orgTest, HTTPOperation.DELETE, client));
            Console.WriteLine(HTTPSCalls.result.Value);
        }

		[Test()]

		public static void createInterceptor()
		{
            //For new version
            //InterceptorJSON json = new InterceptorJSON(int.Parse(LocationTest.getLocId()), "wat", "wappisk", "AYYYYLMAO");
            LocationTest.getLocId();
            string loc = TestGlobals.locIdCreated;
            Console.WriteLine("Creating intercepter w/ loc:");
            Console.WriteLine(loc);
            InterceptorJSON json = new InterceptorJSON(int.Parse(loc), LocationTest.orgIdPassed, "AYYYYLMAO");
			Interceptor newInt = new Interceptor(TestGlobals.testServer, "NotUsed", json);
			Test mTest = new Test(newInt);
            HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken ();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
			Console.WriteLine(HTTPSCalls.result.Value.ToString());
			Console.WriteLine (LocationTest.getLocId ());
            Console.WriteLine(HTTPSCalls.result.Value.ToString());
            TestGlobals.intIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
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
