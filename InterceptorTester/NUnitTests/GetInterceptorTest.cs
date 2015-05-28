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
    public class GetInterceptorTest
    {
        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }

        [Test()]
        public void runTest()
        {
            JObject data = CreateInterceptor.run().Key;
            APIOperation mOp = new GenericRequest(TestGlobals.testServer, "/api/interceptor", data);
            Test mTest = new Test(mOp);
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.GET));
            Assert.AreEqual("Yes", HTTPSCalls.result.Value);
        }
    }

    [TestFixture()]
    public static class CreateInterceptor
    {
        [Test()]
        public static KeyValuePair<JObject, string> run()
        {
            //JObject data = GetLocation.run().Key;
            //APIOperation mOp = new GenericRequest(TestGlobals.testServer, "/api/interceptor", data);
            //Test mTest = new Test(mOp);
            //AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST));

            //TODO: Ensure this is correct
            Assert.AreEqual("201", HTTPSCalls.result.Value);

            return HTTPSCalls.result;
        }
    }

    public class GetLocation
    {

    }

    public class CreateLocation
    {

    }

    public class GetOrganization
    {

    }

    public class CreateOrganization
    {

    }
}
