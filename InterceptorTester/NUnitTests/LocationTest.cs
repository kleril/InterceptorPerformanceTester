﻿using System;
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
<<<<<<< HEAD

		[TestFixtureSetUp()]
	
=======
		public static string locIdCreated;

        [TestFixtureSetUp()]
>>>>>>> origin/master
        public void setup()
        {
            TestGlobals.setup();
        }
<<<<<<< HEAD

        [Test()]	
		 public static void createLocation()
=======
        public static void createLocation()
>>>>>>> origin/master
        {
            LocationJSON json = new LocationJSON(OrganizationTest.getOrgId(), "suite", "street", "suddenValley", "um", "Murica", "A2A2A2");
            json.locDesc = "desc";
            json.locSubType = "subtype";
            json.locType = "type";
<<<<<<< HEAD

			Location newLoc = new Location(TestGlobals.testServer, json);

=======
			Location newLoc = new Location(TestGlobals.testServer, json);
>>>>>>> origin/master
            Test mTest = new Test(newLoc);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
<<<<<<< HEAD

            //Assert.AreEqual("201", HTTPSCalls.result.Value);
            Console.WriteLine(HTTPSCalls.result.Value);
            TestGlobals.locIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);

=======
            Assert.AreEqual("201", HTTPSCalls.result.Value);
            //Assert.AreEqual("201", HTTPSCalls.result.Value);
            Console.WriteLine(HTTPSCalls.result.Value);
            locIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
>>>>>>> origin/master
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

        public static string getLocId()
        {
            if (TestGlobals.locIdCreated == null)
            {
                createLocation();
            }
            return TestGlobals.locIdCreated;
        }
    }
}




