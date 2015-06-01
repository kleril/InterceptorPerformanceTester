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

		[Test()]
		// Invalid Serial
		public void InvalidSerial() 
		{
			DeviceSetting dSetting2 = new DeviceSetting(testServer, invalidSerial);

			Test BadSerial = new Test(dSetting2);
			BadSerial.setTestName("BadSerial");


			List<Test> tests = new List<Test>();
			tests.Add(BadSerial);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}

		[Test()]
		// No Serial
		public void NoSerial() 
		{
			DeviceSetting dSetting3 = new DeviceSetting(testServer, null);

			Test NoSerial = new Test(dSetting3);
			NoSerial.setTestName("NoSerial");


			List<Test> tests = new List<Test>();
			tests.Add(NoSerial);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}
	}
	*/
}

