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
using ConsoleApplication1;

namespace InterceptorTester.Tests.AdminTests
{
	[TestFixture()]
    public class InterceptorTest
    {
        KeyValuePair<JObject, string> intStore;

		[TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
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
            Interceptor newInt = new Interceptor(TestGlobals.adminServer, "NotUsed", json);
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
		public void getSingleInterceptor()
		{
			string query = "/API/Interceptor/" + TestGlobals.validSerial;
            GenericRequest getInt = new GenericRequest(TestGlobals.adminServer, query, null);
			Test mTest = new Test(getInt);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			intStore = HTTPCalls.result;
		}

		[Test()]
		public void getMultipleInterceptors()
		{
			string query = "/API/Interceptor/?LocId=" + TestGlobals.locIdCreated;
            GenericRequest getInt = new GenericRequest(TestGlobals.adminServer, query, null);
			Test mTest = new Test (getInt);
			HttpClient client = new HttpClient ();
			AsyncContext.Run (async() => await new HTTPSCalls ().runTest (mTest, HTTPOperation.GET, client));
			Assert.AreEqual ("201", HTTPCalls.result.Value);
			intStore = HTTPCalls.result;
		}

		[Test()]
		public void deleteInterceptor()
		{
			string query = "/api/interceptor/" + TestGlobals.intIdCreated;
            GenericRequest intReq = new GenericRequest(TestGlobals.adminServer, query, null);
			Test intTest = new Test(intReq);
			HttpClient client;

			client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken(); 
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(intTest, HTTPOperation.DELETE, client));
			Console.WriteLine(HTTPSCalls.result.Value);
		}

	}
}
