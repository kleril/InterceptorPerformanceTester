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
using ConsoleApplication1;
using Newtonsoft.Json.Linq;

namespace InterceptorTester.Tests.PerformanceTests
{
	[TestFixture()]
	public class DeviceScanPerformanceTest
    {
        static StreamWriter results;
		static StreamWriter log;

        static string outputFileHTTPSSync = "../../../logs/SyncHTTPSDeviceScanPerformanceTest.csv";
        static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceScanPerformanceTest.csv";
        static string outputFileHTTPSync = "../../../logs/SyncHTTPDeviceScanPerformanceTest.csv";
        static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceScanPerformanceTest.csv";
        static string outputFileMultiClientScans = "../../../logs/MultiClientDeviceScan.csv";

		static string outputFile = "../../../logs/DeviceScanPerformance.txt";

        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();

			FileStream stream;
			stream = File.Create (outputFile);
			log = new StreamWriter (stream);
        }

		[TestFixtureTearDown()]
		public void tearDown()
		{
			log.Close();
		}

        [Test()]
        public void SyncHTTPSDeviceScan()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSSync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPSDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                DeviceScanJSON testJson = new DeviceScanJSON();
                testJson.i = TestGlobals.validSerial;
                testJson.d = "1289472198573";
                testJson.b = null;
				testJson.s = i;
                DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

                Test scanTest = new Test(testDScan);
                scanTest.setTestName("ValidSingleScanSimple");
				scanTest.setExpectedResult ("201");
				scanTest.setType ("performance");

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
				AsyncContext.Run(async () => await new HTTPSCalls().runTest(scanTest, HTTPOperation.POST));
                timer.Stop();

				JObject posted = JObject.FromObject (testJson);
				log.WriteLine ("Json Posted:");
				log.WriteLine (posted.ToString ());

                double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);
				log.WriteLine ("Test Lasted: " + time + "ms");

				if (i < 99) 
				{
					log.WriteLine ();
				}


                System.Threading.Thread.Sleep(TestGlobals.delay);
                //Verify Server didn't throw up
            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: SyncHTTPSDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        // Valid Single Scan
        public void AsyncHTTPSDeviceScan()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
			JObject[] scanPosted = new JObject[TestGlobals.maxReps];


			log.WriteLine ("Test Started: AsyncHTTPSDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            
            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                // thread sleep?  

                System.Threading.Thread.Sleep(TestGlobals.delay);

				DeviceScanJSON testJson = new DeviceScanJSON();
				testJson.i = TestGlobals.validSerial;
				testJson.d = "1289472198573";
				testJson.b = null;
				testJson.s = i;
				DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

				Test scanTest = new Test(testDScan);
				scanTest.setTestName("ValidSingleScanSimple");
				scanTest.setExpectedResult ("201");
				scanTest.setType ("performance");

				List<Test> tests = new List<Test>();
				tests.Add(scanTest);

                tasks[i] = new HTTPSCalls().runTest(scanTest, HTTPOperation.POST);
				testStarted [i] = HTTPSCalls.started;
				JObject Json = JObject.FromObject(testJson);
				scanPosted[i] = Json;

                Console.WriteLine("Test starting:" + i.ToString());
            }
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("All tests initialized, waiting on them to run as async");
            Console.WriteLine("------------------------------------------------------");
            Task.WaitAll(tasks);

            
			int seq = 0;
			foreach (Task<double> nextResult in tasks)
            {
                results.WriteLine("Test Time," + nextResult.Result);


				log.WriteLine ("Test " + seq + " Started at " + testStarted[seq].ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (scanPosted[seq].ToString ());
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}

            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPSDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");


            results.Close();
        }

        [Test()]
        public void SyncHTTPDeviceScan()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);


            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                DeviceScanJSON testJson = new DeviceScanJSON();
                testJson.i = TestGlobals.validSerial;
                testJson.d = "1289472198573";
                testJson.b = null;
				testJson.s = i;
                DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

                Test scanTest = new Test(testDScan);
                scanTest.setTestName("ValidSingleScanSimple");
				scanTest.setExpectedResult ("201");
				scanTest.setType ("performance");

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
				AsyncContext.Run(async () => await new HTTPCalls().runTest(scanTest, HTTPOperation.POST));
                timer.Stop();

				JObject posted = JObject.FromObject (testJson);
				log.WriteLine ("Json Posted:");
				log.WriteLine (posted.ToString ());

				double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);
				log.WriteLine ("Test Lasted: " + time + "ms");

				if (i < 99) 
				{
					log.WriteLine ();
				}


                System.Threading.Thread.Sleep(TestGlobals.delay);
                //Verify Server didn't throw up
            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;


			log.WriteLine ("Test Ended: SyncHTTPDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");


            results.Close();
        }

        [Test()]
        // Valid Single Scan
        public void AsyncHTTPDeviceScan()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: AsyncHTTPDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

           
            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
			JObject[] scanPosted = new JObject[TestGlobals.maxReps];


            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);

				DeviceScanJSON testJson = new DeviceScanJSON();
				testJson.i = TestGlobals.validSerial;
				testJson.d = "1289472198573";
				testJson.b = null;
				testJson.s = i;
				DeviceScan testDScan = new DeviceScan(TestGlobals.testServer, testJson);

				Test scanTest = new Test(testDScan);
				scanTest.setTestName("ValidSingleScanSimple");
				scanTest.setExpectedResult ("201");
				scanTest.setType ("performance");

				List<Test> tests = new List<Test>();
				tests.Add(scanTest);

                tasks[i] = new HTTPCalls().runTest(scanTest, HTTPOperation.POST);
				testStarted [i] = HTTPCalls.started;
				JObject Json = JObject.FromObject(testJson);
				scanPosted[i] = Json;

                Console.WriteLine("Test starting:" + i.ToString());
            }
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("All tests initialized, waiting on them to run as async");
            Console.WriteLine("------------------------------------------------------");
            Task.WaitAll(tasks);

			int seq = 0;
			foreach (Task<double> nextResult in tasks)
            {
                results.WriteLine("Test Time," + nextResult.Result);

				log.WriteLine ("Test " + seq + " Started at " + testStarted[seq].ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (scanPosted[seq].ToString ());
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}
            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        //Multi-client simultaneious scans
        public void MultiClientScans()
        {
            FileStream stream;
            stream = File.Create(outputFileMultiClientScans);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: MultiClientDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Server: " + TestGlobals.testServer);

            // Construct started tasks
            Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps, 2];

			DateTime[] testStarted1 = new DateTime[TestGlobals.maxReps];
			DateTime[] testStarted2 = new DateTime[TestGlobals.maxReps];
			JObject[] scanPosted1 = new JObject[TestGlobals.maxReps];
			JObject[] scanPosted2 = new JObject[TestGlobals.maxReps];

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);

				// client 1
				DeviceScanJSON testJson1 = new DeviceScanJSON();
				testJson1.i = TestGlobals.validSerial;
				testJson1.d = "1289472198573";
				testJson1.b = null;
				testJson1.s = i;
				DeviceScan Scan1 = new DeviceScan(TestGlobals.testServer, testJson1);

				Test scanTest1 = new Test(Scan1);
				scanTest1.setTestName("ValidSingleScanSimple");
				scanTest1.setExpectedResult ("201");
				scanTest1.setType ("performance");

                tasks[i, 0] = new HTTPCalls().runTest(scanTest1, HTTPOperation.POST);
				testStarted1[i] = HTTPCalls.started;
				JObject Json1 = JObject.FromObject(testJson1);
				scanPosted1[i] = Json1;



				// client 2
				DeviceScanJSON testJson2 = new DeviceScanJSON();
				testJson2.i = TestGlobals.validSerial;
				testJson2.d = "1289472198573";
				testJson2.b = null;
				testJson2.s = i;
				DeviceScan Scan2 = new DeviceScan(TestGlobals.testServer, testJson2);

				Test scanTest2 = new Test(Scan2);
				scanTest2.setTestName("ValidSingleScanSimple");
				scanTest2.setExpectedResult ("201");
				scanTest2.setType ("performance");

                tasks[i, 1] = new HTTPCalls().runTest(scanTest2, HTTPOperation.POST);
				testStarted2[i] = HTTPCalls.started;
				JObject Json2 = JObject.FromObject(testJson2);
				scanPosted2[i] = Json2;

                Console.WriteLine("Test starting:" + i.ToString());
                Task.WaitAll(tasks[i, 0], tasks[i, 1]);
            }

			log.WriteLine ("Client 1:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 0].Result);

				log.WriteLine ("Client 1 Test " + i + " Started at " + testStarted1 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (scanPosted1 [i].ToString ());
				log.WriteLine ("Test Lasted: " + tasks[i, 0].Result + "ms");
				log.WriteLine ();
			}

			log.WriteLine ("Client 2:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 1].Result);

				log.WriteLine ("Client 2 Test " + i + " Started at " + testStarted2 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (scanPosted2 [i].ToString ());
				log.WriteLine ("Test Lasted: " + tasks[i, 1].Result + "ms");
				if (i < 99) 
				{
					log.WriteLine ();
				}
			}

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: MultiClientDeviceScan");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

    }
}
