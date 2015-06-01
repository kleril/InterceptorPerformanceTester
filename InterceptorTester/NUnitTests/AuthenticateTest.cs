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

namespace ConsoleApplication1
{
	[TestFixture()]
	public class AuthenticateTest
    {
        JObject sessionToken;

        [TestFixtureSetUp()]
        public void testSetup()
        {
            TestGlobals.setup();
        }

        [Test()]
        public JObject generateSessionToken()
        {
            AuthenticateJSON json = new AuthenticateJSON();
            //Set up JSON
            json.username = TestGlobals.username;
            json.password = TestGlobals.password;
            Authenticate authCall = new Authenticate(TestGlobals.testServer, TestGlobals.validSerial, json);
            Test authTest = new Test(authCall);
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(authTest, HTTPOperation.POST));
            sessionToken = HTTPSCalls.result.Key;
            return sessionToken;
        }

        public JObject getSessionToken()
        {
            return sessionToken;
        }
	}
}


