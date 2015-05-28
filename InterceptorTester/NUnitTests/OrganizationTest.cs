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

namespace ConsoleApplication1
{
    
	[TestFixture()]
	public class OrganizationTest
    {
        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();
        }

        [Test()]
        public void createOrganization()
        {
            Organization newOrg = new Organization(TestGlobals.testServer, "999");
            Test mTest = new Test(newOrg);
            AsyncContext.Run(async () => await new HTTPSCalls().runTest(mTest, HTTPOperation.POST));
            Assert.AreEqual("201", HTTPSCalls.result.Value);
        }
	}
}

