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
	[TestFixture()]
	public class OrganizationTest
    {
<<<<<<< HEAD
=======
		static string orgIdCreated;

>>>>>>> origin/master
		[TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }

        [Test()]
        public static void createOrganization()
        {
            OrganizationJSON json = new OrganizationJSON();
			json.ownerID = 999;
            json.name = "TestName";
            Organization newOrg = new Organization(TestGlobals.testServer, json);
            Test mTest = new Test(newOrg);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
            //client.setup;
			client.DefaultRequestHeaders.Authorization = AuthenticateTest.getSessionToken();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST, client));
<<<<<<< HEAD
			TestGlobals.orgIdCreated = HTTPSCalls.result.Value.Substring(9, 4);
		
=======
            Console.WriteLine(HTTPSCalls.result.Value);
            orgIdCreated = HTTPSCalls.result.Value.Substring(9, HTTPSCalls.result.Value.Length - 10);
>>>>>>> origin/master
        }

        [Test()]
        public KeyValuePair<JObject, string> getSingleOrganization()
        {
			string query = "/API/Organization/" + TestGlobals.orgIdCreated;
			GenericRequest getOrg = new GenericRequest(TestGlobals.testServer, query, null);
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
			GenericRequest getOrg = new GenericRequest(TestGlobals.testServer, query, null);
			Test mTest = new Test(getOrg);
			HttpClient client = new HttpClient();
			AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET, client));
			Assert.AreEqual("201", HTTPSCalls.result.Value);
			return HTTPCalls.result;
		}

        public static string getOrgId()
        {
            if (orgIdCreated == null)
            {
                createOrganization();
            }
            return orgIdCreated;
        }
	}
}

