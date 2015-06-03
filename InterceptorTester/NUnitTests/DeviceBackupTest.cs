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
	public class DeviceBackupTest
	{
		// Globals

		static StreamWriter results;

		static string outputFileSync = "../../../logs/SyncDeviceBackupTestPerformanceTest.csv";
		static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceBackupTestPerformanceTest.csv";
		static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceBackupTestPerformanceTest.csv";

		[TestFixtureSetUp()]
		public void setup()
		{
			TestGlobals.setup();
		}

		[Test()]
		// Valid Single Backup Item
		public void AsyncHTTPSDeviceBackup()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSAsync);
			results = new StreamWriter(stream);

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
			// Construct started tasks
			Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPSCalls().runTest(backupTest, HTTPOperation.POST);
				Console.WriteLine("Test starting:" + i.ToString());
			}
			Console.WriteLine("------------------------------------------------------");
			Console.WriteLine("All tests initialized, waiting on them to run as async");
			Console.WriteLine("------------------------------------------------------");
			Task.WaitAll(tasks);

			foreach (Task<double> nextResult in tasks)
			{
				results.WriteLine("Test Time," + nextResult.Result);
			}
			results.Close();
		}

		[Test()]
		// Valid Single Backup Item
		public void AsyncHTTPDeviceBackup()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPAsync);
			results = new StreamWriter(stream);

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
			// Construct started tasks
			Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPCalls().runTest(backupTest, HTTPOperation.POST);
				Console.WriteLine("Test starting:" + i.ToString());
			}
			Console.WriteLine("------------------------------------------------------");
			Console.WriteLine("All tests initialized, waiting on them to run as async");
			Console.WriteLine("------------------------------------------------------");
			Task.WaitAll(tasks);

			foreach (Task<double> nextResult in tasks)
			{
				results.WriteLine("Test Time," + nextResult.Result);
			}
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
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(backupTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(failingTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(failingTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
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


			AsyncContext.Run(async () => await new HTTPSCalls().runTest(emptyTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
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

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(serialTest, HTTPOperation.POST));
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
		}

       

		//TODO: Do this in a cleaner way
		public BackupItem getBackupItem(int i)
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

