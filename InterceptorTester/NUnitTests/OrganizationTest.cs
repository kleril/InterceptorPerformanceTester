using System;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace ConsoleApplication1
{
	/*

	public static string server;
	
	[TestFixture()]
	public class OrganizationTest
    {
		[TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
			server = new Uri(ConfigurationManager.ConnectionStrings["IntOpServer"].ConnectionString);
        }

        [Test()]
        public static void createOrganization()
        {
            OrganizationJSON json = new OrganizationJSON(999, "TestName");
			Organization newOrg = new Organization(server, json);
            Test mTest = new Test(newOrg);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            //client.setup;
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
			Console.WriteLine(HTTPSCalls.result.Value);
            TestGlobals.orgIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
        }

        [Test()]
        public KeyValuePair<JObject, string> getSingleOrganization()
        {
			string query = "/API/Organization/" + TestGlobals.orgIdCreated;
			GenericRequest getOrg = new GenericRequest(server, query, null);
            Test mTest = new Test(getOrg);
            HttpClient client = new HttpClient();
            //TODO: Initialize the client properly - add session token to header, etc.
            //client.setup;
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
            Assert.AreEqual("201", HTTPSCalls.result.Value);
            return HTTPCalls.result;
        }

		[Test()]
		public KeyValuePair<JObject, string> getMultipleOrganization()
		{
			string query = "/API/Organization/";
			GenericRequest getOrg = new GenericRequest(server, query, null);
			Test mTest = new Test(getOrg);
			HttpClient client = new HttpClient();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}

		[Test()]
		public void deleteOrganization()
		{
			string query = "/api/organization/" + TestGlobals.orgIdCreated;
			GenericRequest orgReq = new GenericRequest(server, query, null);
			Test orgTest = new Test(orgReq);
			HttpClient client;

			client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken(); 
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(orgTest, HTTPOperation.DELETE, client));
			Console.WriteLine(HTTPSCalls.result.Value);
		}

        public static string getOrgId()
        {
            if (TestGlobals.orgIdCreated == null)
            {
                createOrganization();
            }
            return TestGlobals.orgIdCreated;
        }
	}
	*/
}

