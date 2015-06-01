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
<<<<<<< HEAD

        [Test()]	
		 public static void createLocation()
		{
            LocationJSON json = new LocationJSON(OrganizationTest.getOrgId(), "suite", "street", "Toronto", "Ontario", "Canada", "A2A2A2");
            json.locDesc = "desc";
            json.locSubType = "subtype";
            json.locType = "type";
			json.latitude = 0;
			json.longitude = 0;

=======
        public static void createLocation()
        {
            orgIdPassed = OrganizationTest.getOrgId();
            LocationJSON json = new LocationJSON(orgIdPassed, "suite", "street", "suddenValley", "um", "Murica", "A2A2A2");
            json.locDesc = "desc";
            json.locSubType = "subtype";
            json.locType = "type";
>>>>>>> origin/master
			Location newLoc = new Location(TestGlobals.testServer, json);
            Test mTest = new Test(newLoc);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
            //Assert.AreEqual("201", HTTPSCalls.result.Value);
            Console.WriteLine(HTTPSCalls.result.Value);
<<<<<<< HEAD
            TestGlobals.locIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
			Console.WriteLine (TestGlobals.locIdCreated);
=======
            locIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
>>>>>>> origin/master
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

        public static string getLocId()
        {
            if (locIdCreated == null)
            {
                createLocation();
            }
            return locIdCreated;
        }
    }
}




