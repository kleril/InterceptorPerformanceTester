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
	public class AuthenticateTest
    {
        static JObject sessionToken;

        [TestFixtureSetUp()]
        public void testSetup()
        {
            TestGlobals.setup();
        }

<<<<<<< HEAD
		[Test()]
        public JObject generateSessionToken()
=======
        [TestCase(Result="Somethin'")]
        public static JObject generateSessionToken()
>>>>>>> origin/master
        {
            AuthenticateJSON json = new AuthenticateJSON();
            //Set up JSON
            json.userID = TestGlobals.username;
            json.password = TestGlobals.password;
            Authenticate authCall = new Authenticate(TestGlobals.testServer, TestGlobals.validSerial, json);
            Test authTest = new Test(authCall);
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(authTest, HTTPOperation.POST));
			sessionToken = JObject.Parse(HTTPSCalls.result.Value);
            return sessionToken;
        }

        [Test()]
        public void closeSession()
        {
            if (sessionToken != null)
            {
                GenericRequest req = new GenericRequest(TestGlobals.testServer, "/api/authenticate/", null);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = getSessionToken();
                Test closeTest = new Test(req);
                AsyncContext.Run(async () => await new HTTPSCalls().runTest(closeTest, HTTPOperation.DELETE, client));
            }
        }

<<<<<<< HEAD
		[Test()]
		public AuthenticationHeaderValue getSessionToken()
=======
        public static AuthenticationHeaderValue getSessionToken()
>>>>>>> origin/master
        {
            if (sessionToken == null)
            {
                Console.WriteLine("No Session token found. Generating session token...");
                generateSessionToken();
            }
            if (sessionToken.GetValue("_sessionToken") == null)
            {
                Console.WriteLine("Session token is empty for some reason");
                generateSessionToken();
            }
            string parse = "Token " + sessionToken.GetValue("_sessionToken").ToString();
            AuthenticationHeaderValue ret = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(parse);
            return ret;
        }
	}
}