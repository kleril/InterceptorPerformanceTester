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
        public JObject generateSessionToken()
        {
            AuthenticateJSON json = new AuthenticateJSON();
            //Set up JSON
            json.username = "username";
            json.password = "password";
            Authenticate authCall = new Authenticate(TestGlobals.testServer, TestGlobals.validSerial, json);
            Test authTest = new Test(authCall);
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(authTest, HTTPOperation.POST));
            return HTTPSCalls.result.Key;
        }
	}
}


