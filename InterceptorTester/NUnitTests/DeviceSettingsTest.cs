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
	/*
	[TestFixture()]
	public class DeviceSettingsTest
    {
        static Uri testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
        static string validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
        static string invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;

		[Test()]
		// Valid Serial
		public void ValidSerial() 
		{
			DeviceSetting dSetting1 = new DeviceSetting(testServer, validSerial);

			Test ValidSerial = new Test(dSetting1);
			ValidSerial.setTestName("ValidSerial");


			List<Test> tests = new List<Test>();
			tests.Add(ValidSerial);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}
	}
	*/
}

