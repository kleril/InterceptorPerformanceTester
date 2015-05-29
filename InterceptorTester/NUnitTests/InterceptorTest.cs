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
    //TODO: Load globals in a cleaner way
    public static class TestGlobals
    {
        public static Uri testServer;
        public static string validSerial;
        public static string invalidSerial;
        public static int delay;

        public static void setup()
        {
            try
            {
                testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
                validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
                invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;
                delay = int.Parse(ConfigurationManager.ConnectionStrings["DelayBetweenRuns"].ConnectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

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
		public KeyValuePair<JObject, string> getIntercpetor()
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
	}

}
