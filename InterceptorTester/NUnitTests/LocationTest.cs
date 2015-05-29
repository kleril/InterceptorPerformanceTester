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
        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }

        [Test()]
        public KeyValuePair<JObject, string> createLocation()
        {
            //TODO: Set this up
            Location newLoc = new Location(TestGlobals.testServer, "");
            Test mTest = new Test(newLoc);
            HttpClient client = new HttpClient();
            //TODO: Initialize the client properly - add session token to header, etc.
            //client.setup;
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
            Assert.AreEqual("201", HTTPSCalls.result.Value);
            return HTTPCalls.result;
        }

        [Test()]
        public KeyValuePair<JObject, string> getOrganization()
        {
            GenericRequest getLoc = new GenericRequest(TestGlobals.testServer, "/API/Organization/999", null);
            Test mTest = new Test(getLoc);
            HttpClient client = new HttpClient();
            //TODO: Initialize the client properly - add session token to header, etc.
            //client.setup;
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
            Assert.AreEqual("201", HTTPSCalls.result.Value);
            return HTTPCalls.result;
        }
    }
	/*
	[TestFixture()]
	public class LocationTest
    {
        
	}
	*/
}




