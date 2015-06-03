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

namespace ConsoleApplication1
{
	
	[TestFixture()]
    class DeviceScanTest
    {

        static StreamWriter results;

        [TestFixtureSetUp()]
        static public void setup()
        {
            TestGlobals.setup ();
        }

        static string outputFileHTTPSSync = "../../../logs/SyncHTTPSDeviceScanPerformanceTest.csv";
        static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceScanPerformanceTest.csv";
		static string outputFileHTTPSync = "../../../logs/SyncHTTPDeviceScanPerformanceTest.csv";
        static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceScanPerformanceTest.csv";
        static string outputFileMultiClientScans = "../../../logs/MultiClientScans.csv";

        // simple scan code

		[Test()]
		public void SyncHTTPSDeviceScan()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSSync);
			results = new StreamWriter(stream);

			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
				DeviceScanJSON testJson = new DeviceScanJSON ();
				testJson.i = TestGlobals.validSerial;
				testJson.d = "1289472198573";
				testJson.b = null;
				testJson.s = 4;
				DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

				Test scanTest = new Test(testDScan);
				scanTest.setTestName("ValidSingleScanSimple");

				timer.Start();
				AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.GET));
				timer.Stop();
				double time = timer.Elapsed.TotalMilliseconds;
				results.WriteLine("Test Time," + time);
				System.Threading.Thread.Sleep (TestGlobals.delay);
				//Verify Server didn't throw up
			}
			results.Close();
		}

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
			
		[Test()]
		public void SyncHTTPDeviceScan()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSync);
			results = new StreamWriter(stream);

			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
				DeviceScanJSON testJson = new DeviceScanJSON ();
				testJson.i = TestGlobals.validSerial;
				testJson.d = "1289472198573";
				testJson.b = null;
				testJson.s = 4;
				DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

				Test scanTest = new Test(testDScan);
				scanTest.setTestName("ValidSingleScanSimple");

				timer.Start();
				AsyncContext.Run(async () => await new HTTPCalls().runTest(scanTest, HTTPOperation.GET));
				timer.Stop();
				double time = timer.Elapsed.TotalMilliseconds;
				results.WriteLine("Test Time," + time);
				System.Threading.Thread.Sleep (TestGlobals.delay);
				//Verify Server didn't throw up
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

        [Test()]
        //Multi-client simultaneious scans
        public void multiClientScans()
        {
            FileStream stream;
            stream = File.Create(outputFileMultiClientScans);
            results = new StreamWriter(stream);

            DeviceScanJSON testJson1 = new DeviceScanJSON();
            testJson1.i = TestGlobals.validSerial;
            testJson1.d = "1289472198573";
            testJson1.b = null;
            testJson1.s = 4;
            DeviceScan Scan1 = new DeviceScan(TestGlobals.testServer, testJson1);

            Test scanTest1 = new Test(Scan1);
            scanTest1.setTestName("ValidSingleScanSimple");


            DeviceScanJSON testJson2 = new DeviceScanJSON();
            testJson2.i = TestGlobals.validSerial;
            testJson2.d = "1289472198573";
            testJson2.b = null;
            testJson2.s = 4;
            DeviceScan Scan2 = new DeviceScan(TestGlobals.testServer, testJson2);

            Test scanTest2 = new Test(Scan2);
            scanTest2.setTestName("ValidSingleScanSimple");

            // Construct started tasks
            Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps,2];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i,0] = new HTTPCalls().runTest(scanTest1, HTTPOperation.POST);
                tasks[i,1] = new HTTPCalls().runTest(scanTest2, HTTPOperation.POST);
                Console.WriteLine("Test starting:" + i.ToString());
                Task.WaitAll(tasks[i,0], tasks[i,1]);
            }

            foreach (Task<double> nextResult in tasks)
            {
                results.WriteLine("Test Time," + nextResult.Result);
            }

            results.Close();
        }


		// Simple Scan Data

        [Test()]
        // Valid Single Scan
        public void ValidSingleScanSimple()
        {

            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = "1289472198573";
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("ValidSingleScanSimple");


            AsyncContext.Run(async() => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        [Test()]
        public void UTF8ScanCode()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = "¿ÀÁÂÆÐ123òü";
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("UTF8ScanCode");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        [Test()]
        public void ASCIIScanCode()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = "!\"#&)(";
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("ASCIIScanCode");

			AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
			string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
			Assert.AreEqual("201", statusCode);
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
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("InvalidSerialSimple");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
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
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("EmptySerialSimple");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
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
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("NullSerialSimple");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
        }

        [Test()]
        // List of Valid Scans
        public void LOValidScansSimple()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = null;
            string[] scanData = new string[4];
            scanData [0] = "0";
            scanData [1] = "1";
            scanData [2] = "2";
            scanData [3] = "3";
            testJson.b = scanData;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

            Test scanTest = new Test(testDScan);
            scanTest.setTestName("LOValidScansSimple");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        //Dynamic

        [Test()]
        // Valid Single Scan
        public void ValidSingleScanDyn()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = "~20/90210|";
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("ValidSingleScanDyn");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        [Test()]
        // Valid Call Home Scan
        public void ValidCH()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = "~21/*CH*055577520928";
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("ValidCH");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        //TODO: Identify invalid possible scans
        /*
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
        */

        [Test()]
        // Bad Serial
        public void InvalidSerialDyn()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.invalidSerial;
            testJson.d = "~20/90210|";
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("InvalidSerialDyn");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
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
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("EmptySerialDyn");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
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
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("NullSerialDyn");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
        }

        [Test()]
        // List of Valid Scans
        public void LOValidScansDyn()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = null;
            string[] scanData = new string[4];
            scanData[0] = "~20/0|";
            scanData[1] = "~20/1|";
            scanData[2] = "~20/2|";
            scanData[3] = "~20/3|";
            testJson.b = scanData;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("LOValidScansDyn");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        //TODO: Identify invalid scan
        /*
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

        */

        // Combined

        [Test()]
        // List of Valid Simple and Dynamic Code Scans 
        public void ValidScansSimDyn()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = null;
            string[] scanData = new string[4];
            scanData [0] = "~20/0|";
            scanData [1] = "123456789";
            scanData [2] = "~20/2|";
            scanData [3] = "987654321";
            testJson.b = scanData;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("ValidScansSimDyn");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("201", statusCode);
        }

        //TODO: Need dynamic invalid scan
        /*
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
        */

        [Test()]
        // No scan data
        public void NoScanData()
        {
            DeviceScanJSON testJson = new DeviceScanJSON ();
            testJson.i = TestGlobals.validSerial;
            testJson.d = null;
            testJson.b = null;
            testJson.s = 4;
            DeviceScan testDScan = new DeviceScan (TestGlobals.testServer, testJson);

            Test scanTest = new Test (testDScan);
            scanTest.setTestName("NoScanData");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
        }
    }
}
