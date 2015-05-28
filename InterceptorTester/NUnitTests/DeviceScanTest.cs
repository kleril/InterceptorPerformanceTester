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
	public class DeviceScanTest
	{
		static StreamWriter results;
		public int maxReps;

		static Uri testServer;
		static string validSerial;
		static string invalidSerial;
		static int delay;


		[TestFixtureSetUp()]
		public void setup()
		{
			try
			{
				testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
				validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
				invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;
				delay = int.Parse(ConfigurationManager.ConnectionStrings["DelayBetweenRuns"].ConnectionString);

				string testRunsString = ConfigurationManager.ConnectionStrings["TimesToRunTests"].ConnectionString;
				try { maxReps = int.Parse(testRunsString); }
				catch (Exception e)
				{
					Console.WriteLine(e);
					Console.WriteLine("Chances are your appconfig is misconfigured. Double check that performanceTestRuns is an integer and try again.");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		static string outputFileSync = "../../../logs/SyncDeviceScanPerformanceTest.csv";
		static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceScanPerformanceTest.csv";
		static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceScanPerformanceTest.csv";

		// simple scan code

		[Test()]
		// Valid Single Scan
		public void AsyncHTTPSDeviceScan()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSAsync);
			results = new StreamWriter(stream);

			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("ValidSingleScanSimple");

			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[maxReps];
			for (int i = 0; i < maxReps; i++)
			{
				System.Threading.Thread.Sleep(delay);
				tasks[i] = new HTTPSCalls().runTest(scanTest);
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
		// Valid Single Scan
		public void AsyncHTTPDeviceScan()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPAsync);
			results = new StreamWriter(stream);

			DeviceScanJSON testJson = new DeviceScanJSON();
			testJson.i = validSerial;
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("ValidSingleScanSimple");

			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[maxReps];
			for (int i = 0; i < maxReps; i++)
			{
				System.Threading.Thread.Sleep(delay);
				tasks[i] = new HTTPCalls().runTest(scanTest);
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


		/*
		[Test()]
		// List of Valid Scans
		public void LOValidScansSimple()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			string[] scanData = new string[4];
			scanData [0] = "0";
			scanData [1] = "1";
			scanData [2] = "2";
			scanData [3] = "3";
			testJson.b = scanData;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("LOValidScansSimple");


			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}            
		}

		//Dynamic

		[Test()]
		// Valid Single Scan
		public void ValidSingleScanDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = "~20/90210|";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("ValidSingleScanDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) 
			{
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}

		[Test()]
		// Valid Call Home Scan
		public void ValidCH()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = "~21/*CH*055577520928";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("ValidCH");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}

		[Test()]
		// List of Valid Scans
		public void LOValidScansDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			string[] scanData = new string[4];
			scanData[0] = "~20/0|";
			scanData[1] = "~20/1|";
			scanData[2] = "~20/2|";
			scanData[3] = "~20/3|";
			testJson.b = scanData;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("LOValidScansDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}

		// Combined
		[Test()]
		// List of Valid Simple and Dynamic Code Scans 
		public void ValidScansSimDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			string[] scanData = new string[4];
			scanData [0] = "~20/0|";
			scanData [1] = "123456789";
			scanData [2] = "~20/2|";
			scanData [3] = "987654321";
			testJson.b = scanData;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("ValidScansSimDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}
		*/
	}
}


