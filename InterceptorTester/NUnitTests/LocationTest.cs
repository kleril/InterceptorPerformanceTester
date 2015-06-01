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
		public static string locIdcreated;

		[TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }

        [Test()]
		public void createLocation()
        {
            //TODO: Set up JSON
			LocationJSON locjson = new LocationJSON();
			locjson.orgId = TestGlobals.orgIdCreated;
			locjson.unitSuite = "testsuite";
			locjson.street = "teststreet";
			locjson.city = "testcity";
			locjson.stateProvince = "testprovince";
			locjson.country = "testcountry";
			locjson.postalCode = "testpostalcode";

			Location newLoc = new Location(TestGlobals.testServer, locjson);
            Test mTest = new Test(newLoc);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
            Assert.AreEqual("201", HTTPSCalls.result.Value);
        }

        [Test()]
        public KeyValuePair<JObject, string> getSingleLocation()
        {
			string query = "/API/Location/" + TestGlobals.validLocId;
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
			string query = "/API/Location/?orgid=" + TestGlobals.validOrgId;
			GenericRequest getLoc = new GenericRequest(TestGlobals.testServer, query, null);
			Test mTest = new Test(getLoc);
			HttpClient client = new HttpClient();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}
    }
}




