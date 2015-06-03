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
	
	//[TestFixture()]
	public class DeviceScanTest
	{
		public static Test getScan(int i)
		{
			if (i == 1) {
				DeviceScanJSON scan1 = new DeviceScanJSON ();
				scan1.i = TestGlobals.validSerial;
				scan1.d = "1";
				scan1.b = null;
				scan1.s = 4;
				DeviceScan testDScan1 = new DeviceScan (TestGlobals.testServer, scan1);

				Test scanTest1 = new Test(testDScan1);
				return scanTest1;
			} 
			else if (i == 2)
			{
				DeviceScanJSON scan2 = new DeviceScanJSON ();
				scan2.i = TestGlobals.validSerial;
				scan2.d = "2";
				scan2.b = null;
				scan2.s = 4;
				DeviceScan testDScan2 = new DeviceScan (TestGlobals.testServer, scan2);

				Test scanTest2 = new Test (testDScan2);
				return scanTest2;
			}
			else if (i == 3)
			{
				DeviceScanJSON scan3 = new DeviceScanJSON ();
				scan3.i = TestGlobals.validSerial;
				scan3.d = "2";
				scan3.b = null;
				scan3.s = 4;
				DeviceScan testDScan3 = new DeviceScan (TestGlobals.testServer, scan3);

				Test scanTest3 = new Test (testDScan3);
				return scanTest3;
			}
			else if (i == 4)
			{
				DeviceScanJSON scan4 = new DeviceScanJSON ();
				scan4.i = TestGlobals.validSerial;
				scan4.d = "2";
				scan4.b = null;
				scan4.s = 4;
				DeviceScan testDScan4 = new DeviceScan (TestGlobals.testServer, scan4);

				Test scanTest4 = new Test (testDScan4);
				return scanTest4;
			}
            else
            {
                return null;
            }
		}

		/*
		static StreamWriter results;

		[TestFixtureSetUp()]
		static public void setup()
		{
			TestGlobals.setup ();
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
			testJson.i = TestGlobals.validSerial;
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("ValidSingleScanSimple");

			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				// thread sleep?  

				System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPSCalls().runTest(scanTest, HTTPOperation.POST);
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

		// add realistic test case

		[Test()]
		// Valid Single Scan
		public void AsyncHTTPDeviceScan()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPAsync);
			results = new StreamWriter(stream);

			DeviceScanJSON testJson = new DeviceScanJSON();
			testJson.i = TestGlobals.validSerial;
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("ValidSingleScanSimple");

			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);
				tasks[i] = new HTTPCalls().runTest(scanTest, HTTPOperation.POST);
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
		*/


		/*
		[Test()]
		// Valid Single Scan
		public void ValidSingleScanSimple()
		{

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

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}            
		}

		[Test()]
		public void UTF8ScanCode()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = "¿ÀÁÂÆÐ123òü";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("UTF8ScanCode");

			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}            
		}

		[Test()]
		public void ASCIIScanCode()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = "!\"#&)(";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("ASCIIScanCode");

			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}            
		}

		[Test()]
		// Invalid Single Scan
		public void InvalidSingleScanSimple()
		{
            //TODO: Given when then comments
           // var getThing = appconfig.get(invalidScanData);

			DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = validSerial;
			testJson.d = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("InvalidSingleScanSimple");


			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}     
		}

		[Test()]
		// Bad Serial
		public void InvalidSerialSimple()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = "BAD SERIAL";
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("InvalidSerialSimple");


			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}  
		}

		[Test()]
		// No Serial(Empty String)
		public void EmptySerialSimple()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = "";
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("EmptySerialSimple");


			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}  
		}



		[Test()]
		// No Serial(Null)
		public void NullSerialSimple()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = null;
			testJson.d = "1289472198573";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("NullSerialSimple");


			List<Test> tests = new List<Test>();
			tests.Add(scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}  
		}

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

		[Test()]
		// Mixed of Valid/Invalid Scans
		public void ValInvalScansSimple()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			string[] scanData = new string[4];
			scanData [0] = "0";
			scanData [1] = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";
			scanData [2] = "2";
			scanData [3] = "3";
			testJson.b = scanData;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan(testServer, testJson);

			Test scanTest = new Test(testDScan);
			scanTest.setTestName("ValInvalScansSimple");


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
		// Invalid Scan Data
		public void InvalidSingleScanDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = "~20|noendingbar";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("InvalidSingleScanDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}

		[Test()]
		// Bad Serial
		public void InvalidSerialDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = invalidSerial;
			testJson.d = "~20/90210|";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("InvalidSerialDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}


		[Test()]
		// No Serial (Empty Sting)
		public void EmptySerialDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = "";
			testJson.d = "~20/90210|";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("EmptySerialDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}

		[Test()]
		// No Serial (Null)
		public void NullSerialDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = null;
			testJson.d = "~20/90210|";
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("NullSerialDyn");


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

		[Test()]
		// Mixed of Valid/Invalid Scans
		public void ValInvalScansDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			string[] scanData = new string[4];
			scanData [0] = "~20/0|";
			scanData [1] = "~20/noendingbar";
			scanData [2] = "~20/2|";
			scanData [3] = "~20/3|";
			testJson.b = scanData;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("ValInvalScansDyn");


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

		[Test()]
		// Mixed of Valid and Invalid Scans
		public void ValInvalScansSimDyn()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			string[] scanData = new string[4];
			scanData [0] = "~20/0|";
			scanData [1] = "123456789";
			scanData [2] = "~20/noendingbar";
			scanData [3] = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";;
			testJson.b = scanData;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("ValInvalScansSimDyn");


			List<Test> tests = new List<Test> ();
			tests.Add (scanTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests()) {
				Assert.AreEqual (nextTest.getExpectedResult (), nextTest.getActualResult ());
			}
		}

		[Test()]
		// No scan data
		public void NoScanData()
		{
			DeviceScanJSON testJson = new DeviceScanJSON ();
			testJson.i = validSerial;
			testJson.d = null;
			testJson.b = null;
			testJson.s = 4;
			DeviceScan testDScan = new DeviceScan (testServer, testJson);

			Test scanTest = new Test (testDScan);
			scanTest.setTestName("NoScanData");


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


