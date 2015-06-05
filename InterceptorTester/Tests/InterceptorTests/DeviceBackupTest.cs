﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using System.Configuration;
using Nito.AsyncEx;
using System.IO.Compression;
using ConsoleApplication1;

namespace InterceptorTester.Tests.InterceptorTests
{
	[TestFixture()]
	public class DeviceBackupTest
	{
		// Globals

		static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceBackupTestPerformanceTest.csv";
		static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceBackupTestPerformanceTest.csv";
		static string outputFileHTTPSSync = "../../../logs/SyncHTTPSDeviceBackupTestPerformanceTest.csv";
		static string outputFileHTTPSync = "../../../logs/SyncHTTPDeviceBackupTestPerformanceTest.csv";
		static string outputFileMultiClientDeviceBackup = "../../../logs/MultiClientDeviceBackup.csv";

		static StreamWriter results;

		[TestFixtureSetUp()]
		public void setup()
		{
			TestGlobals.setup();

			FileStream stream;
			stream = File.OpenWrite(TestGlobals.logFile);
			results = new StreamWriter(stream);
		}

		[TestFixtureTearDown()]
		public void tearDown()
		{
			results.Close();
		}
			
        
		[Test()]
		// Valid Serial
		public void ValidSerial()
		{
			BackupItem[] items = new BackupItem[3];
			items[0] = getBackupItem(1);
			items[1] = getBackupItem(2);
			items[2] = getBackupItem(3);

			//BackupJSon
			DeviceBackupJSON json = new DeviceBackupJSON();
            json.i = TestGlobals.validSerial;
			json.s = 4;
			json.b = items;

			//BackupOperation
			DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

			//Test
			Test backupTest = new Test(operation);
			backupTest.setTestName("ValidSerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(backupTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			backupTest.setExpectedResult ("201");
	
			Assert.AreEqual("201", statusCode);
		}

		[Test()]
		// Valid Single Backup Item
		public void ValidSingleBackupItem()
		{
			BackupItem[] items = new BackupItem[1];
			items[0] = getBackupItem(1);

			//BackupJSon
			DeviceBackupJSON json = new DeviceBackupJSON();
			json.i = TestGlobals.validSerial;
			json.s = 4;
			json.b = items;

			//BackupOperation
			DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

			//Test
			Test backupTest = new Test(operation);
			backupTest.setTestName("ValidSingleBackupItem");
			backupTest.setExpectedResult ("201");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(backupTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}
			
		[Test()]
		// Invalid Single Backup Item
		public void InvalidSingleBackupItem()
		{
			BackupItem failItem = new BackupItem();

			BackupItem[] failItems = new BackupItem[1];
			failItems[0] = failItem;

			DeviceBackupJSON failJson = new DeviceBackupJSON();
			failJson.i = TestGlobals.validSerial;
			failJson.s = 5;
			failJson.b = failItems;

			DeviceBackup failOperation = new DeviceBackup(TestGlobals.testServer, failJson);
			Test failingTest = new Test(failOperation);
			failingTest.setTestName("InvalidSingleBackupItem");
			failingTest.setExpectedResult ("400");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(failingTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		// Muliple Backup Items with Invalid Backup Item in Them
		public void InvalidBackupItems()
		{
			BackupItem failItem = new BackupItem();

			BackupItem[] failItems = new BackupItem[4];
			failItems[0] = getBackupItem(1);
			failItems[1] = getBackupItem(2);
			failItems[2] = failItem;
			failItems[3] = getBackupItem(3);

			DeviceBackupJSON failJson = new DeviceBackupJSON();
			failJson.i = TestGlobals.validSerial;
			failJson.s = 5;
			failJson.b = failItems;

			DeviceBackup failOperation = new DeviceBackup(TestGlobals.testServer, failJson);
			Test failingTest = new Test(failOperation);
			failingTest.setTestName("InvalidBackupItems");
			failingTest.setExpectedResult ("400");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(failingTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		// Invalid Serial Number
		public void BadSerial()
		{
			BackupItem[] items = new BackupItem[3];
			items[0] = getBackupItem(1);
			items[1] = getBackupItem(2);
			items[2] = getBackupItem(3);

			DeviceBackupJSON serialJson = new DeviceBackupJSON();
			serialJson.i = TestGlobals.invalidSerial;
			serialJson.s = 6;
			serialJson.b = items;

			DeviceBackup serialOperation = new DeviceBackup(TestGlobals.testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("BadSerial");
			serialTest.setExpectedResult ("400");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}

		[Test()]
		// No Backup Items
		public void NoBackupItems()
		{
			BackupItem[] items = new BackupItem[0];

			DeviceBackupJSON emptyJson = new DeviceBackupJSON();
			emptyJson.i = TestGlobals.validSerial;
			emptyJson.s = 8;
			emptyJson.b = items;

			DeviceBackup emptyOperation = new DeviceBackup(TestGlobals.testServer, emptyJson);
			Test emptyTest = new Test(emptyOperation);
			emptyTest.setTestName("NoBackupItems");
			emptyTest.setExpectedResult ("201");


            AsyncContext.Run(async () => await new HTTPSCalls().runTest(emptyTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}

		[Test()]
		// Input with Serial Number as ""
		public void EmptySerial()
		{
			BackupItem[] items = new BackupItem[3];
			items[0] = getBackupItem(1);
			items[1] = getBackupItem(2);
			items[2] = getBackupItem(3);

			DeviceBackupJSON serialJson = new DeviceBackupJSON();
			serialJson.s = 6;
			serialJson.b = items;
			serialJson.i = "";

			DeviceBackup serialOperation = new DeviceBackup(TestGlobals.testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("EmptySerial");
			serialTest.setExpectedResult ("400");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("400", statusCode);
		}


		[Test()]
		// Input with Null Serial Number
		public void NullSerial()
		{
			BackupItem[] items = new BackupItem[3];
			items[0] = getBackupItem(1);
			items[1] = getBackupItem(2);
			items[2] = getBackupItem(3);

			DeviceBackupJSON serialJson = new DeviceBackupJSON();
			serialJson.s = 6;
			serialJson.b = items;
			serialJson.i = null;

			DeviceBackup serialOperation = new DeviceBackup(TestGlobals.testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("NullSerial");
			serialTest.setExpectedResult ("201");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}

		[Test()]
		// Scan Code being a Special Dynamic Code
		public void SpecialDynCode()
		{
			BackupItem[] items = new BackupItem[1];
			items[0] = new BackupItem();
			items[0].d = "~123~status=ssid|";
			items[0].s = 442;
			items[0].t = new DateTime(2015, 5, 11, 2, 4, 22, 295);
			items[0].c = false;

			DeviceBackupJSON serialJson = new DeviceBackupJSON();
			serialJson.s = 6;
			serialJson.b = items;
			serialJson.i = TestGlobals.validSerial;

			DeviceBackup serialOperation = new DeviceBackup(TestGlobals.testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("SpecialDynCode");
			serialTest.setExpectedResult ("201");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}


		[Test()]
		// Scan Code being a Dynamic Code and Not Special
		public void NotSpecialDynCode()
		{
			BackupItem[] items = new BackupItem[1];
			items[0] = new BackupItem();
			items[0].d = "~20/12345|";
			items[0].s = 442;
			items[0].t = new DateTime(2015, 5, 11, 2, 4, 22, 295);
			items[0].c = false;

			DeviceBackupJSON serialJson = new DeviceBackupJSON();
			serialJson.s = 6;
			serialJson.b = items;
			serialJson.i = TestGlobals.validSerial;

			DeviceBackup serialOperation = new DeviceBackup(TestGlobals.testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("NotSpecialDynCode");
			serialTest.setExpectedResult ("201");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}

		[Test()]
		// Multiple valid backup items with simple and dyn code
		public void ValidBackupItemsSimDyn()
		{
			BackupItem[] items = new BackupItem[2];
			items[0] = new BackupItem();
			items[0].d = "~20/12345|";
			items[0].s = 442;
			items[0].t = new DateTime(2015, 5, 11, 2, 4, 22, 295);
			items[0].c = false;

			items[1] = getBackupItem(1);

			DeviceBackupJSON serialJson = new DeviceBackupJSON();
			serialJson.s = 6;
			serialJson.b = items;
			serialJson.i = TestGlobals.validSerial;

			DeviceBackup serialOperation = new DeviceBackup(TestGlobals.testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("ValidBackupItemsSimDyn");
			serialTest.setExpectedResult ("201");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
            results.WriteLine(HTTPSCalls.result.ToString());
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}

       

		//TODO: Do this in a cleaner way
		public static BackupItem getBackupItem(int i)
		{
			List<BackupItem> items = new List<BackupItem>();
			//BackupItems
			BackupItem item1 = new BackupItem();
			item1.d = "12566132";
			item1.s = 442;
			item1.t = new DateTime(2015, 5, 11, 2, 4, 22, 295);
			item1.c = false;
			BackupItem item2 = new BackupItem();
			item2.d = "534235721";
			item2.s = 442;
			item2.t = new DateTime(2015, 5, 11, 2, 4, 28, 216);
			item2.c = false;
			BackupItem item3 = new BackupItem();
			item3.d = "892535";
			item3.s = 442;
			item3.t = new DateTime(2015, 5, 11, 2, 4, 25, 142);
			item3.c = false;

			items.Add(item1);
			items.Add(item2);
			items.Add(item3);

			return items[i-1];
		}
	}
}

