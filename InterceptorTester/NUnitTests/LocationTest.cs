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
    public class LocationTest
    {
		public static string locIdCreated;
        public static string orgIdPassed;

        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }
        [Test()]
        public static void createLocation()
        {
            orgIdPassed = OrganizationTest.getOrgId();
            LocationJSON json = new LocationJSON(orgIdPassed, "suite", "street", "suddenValley", "um", "Murica", "A2A2A2");
            json.locDesc = "desc";
            json.locSubType = "subtype";
            json.locType = "type";
			Location newLoc = new Location(TestGlobals.testServer, json);
            Test mTest = new Test(newLoc);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
            //Assert.AreEqual("201", HTTPSCalls.result.Value);
            Console.WriteLine(HTTPSCalls.result.Value);
            TestGlobals.locIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
            Console.WriteLine(HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10) + " Written to testGlobals");
        }

        [Test()]
        public KeyValuePair<JObject, string> getSingleLocation()
        {
			string query = "/API/Location/" + TestGlobals.locIdCreated;
			GenericRequest getLoc = new GenericRequest(TestGlobals.testServer, query, null);
            Test mTest = new Test(getLoc);
            HttpClient client = new HttpClient();
            //TODO: Initialize the client properly - add session token to header, etc.
            //client.setup;
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
            Assert.AreEqual("201", HTTPSCalls.result.Value);
            return HTTPCalls.result;
        }

		[Test()]
		public KeyValuePair<JObject, string> getMultipleLocations()
		{
			string query = "/API/Location/?orgid=" + TestGlobals.orgIdCreated;
			GenericRequest getLoc = new GenericRequest(TestGlobals.testServer, query, null);
			Test mTest = new Test(getLoc);
			HttpClient client = new HttpClient();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}

		[Test()]
		public void deleteLocation()
		{
			GenericRequest locReq = new GenericRequest(TestGlobals.testServer, "/api/location/" + TestGlobals.locIdCreated, null);
			Test locTest = new Test(locReq);
			HttpClient client;

			client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken(); 
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(locTest, HTTPOperation.DELETE, client));
			Console.WriteLine(HTTPSCalls.result.Value);
		}


        public static string getLocId()
        {
            if (TestGlobals.locIdCreated == null)
            {
                Console.WriteLine("Could not get loc ID from test globals");
                if (LocationTest.locIdCreated == null)
                {
                    Console.WriteLine("Could not get loc ID from loc test");
                    createLocation();
                    Console.WriteLine("Loc created");
                }
            }
            return TestGlobals.locIdCreated;
        }
    }
}