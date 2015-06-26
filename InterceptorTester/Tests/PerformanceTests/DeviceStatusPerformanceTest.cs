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
	public class DeviceStatusPerformanceTest
    {
        static StreamWriter results;
		static StreamWriter log;

        static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceStatusPerformanceTest.csv";
        static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceStatusPerformanceTest.csv";
		static string outputFileHTTPSSync = "../../../logs/SyncHTTPSDeviceStatusPerformanceTest.csv";
		static string outputFileHTTPSync = "../../../logs/SyncHTTPDeviceStatusPerformanceTest.csv";
		static string outputFileMultiClientStatus = "../../../logs/MultiClientDeviceStatus.csv";

		static string outputFile = "../../../logs/DeviceStatusPerformance.txt";

        DeviceStatusJSON status;

        [TestFixtureSetUp]
        public void setup()
        {
            status = new DeviceStatusJSON();
            status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
            status.callHomeTimeoutData = "";
            status.callHomeTimeoutMode = "0";
            status.capture = "1";
            status.captureMode = "1";
            status.cmdChkInt = "1";
            status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
            string[] err = new string[3];
            err[0] = "asdf";
            err[1] = "wasd";
            err[2] = "qwerty";
            status.dynCodeFormat = err;
            status.errorLog = err;
            status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
            status.requestTimeoutValue = "8000";
            status.revId = "52987";
            status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
            //status.seqNum = "87";
            status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

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
		public void SyncHTTPSDeviceStatus()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSSync);
			results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPSDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

				status.seqNum = i.ToString();

				DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);

				Test statusTest = new Test(operation);
				statusTest.setTestName("ValidSerial");
				statusTest.setExpectedResult ("201");
				statusTest.setType ("performance");

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
				AsyncContext.Run(async () => await new HTTPSCalls().runTest(statusTest, HTTPOperation.POST));
				timer.Stop();

				JObject posted = JObject.FromObject (status);
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

			log.WriteLine ("Test Ended: SyncHTTPSDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

			results.Close();
		}


        [Test()]
        public void AsyncHTTPSDeviceStatus()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
			JObject[] statusPosted = new JObject[TestGlobals.maxReps];

			log.WriteLine ("Test Started: AsyncHTTPSDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);


           
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);

				status.seqNum = i.ToString();
				DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
				Test statusTest = new Test(operation);
				statusTest.setTestName("ValidSerial");
				statusTest.setExpectedResult ("201");
				statusTest.setType ("performance");


				List<Test> tests = new List<Test>();
				tests.Add(statusTest);

                tasks[i] = new HTTPSCalls().runTest(statusTest, HTTPOperation.POST);
				testStarted [i] = HTTPSCalls.started;
				JObject Json = JObject.FromObject(status);
				statusPosted[i] = Json;

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
				log.WriteLine (statusPosted[seq].ToString ());
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}

            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPSDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

		[Test()]
		public void SyncHTTPDeviceStatus()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSync);
			results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

				status.seqNum = i.ToString();
				DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);

				Test statusTest = new Test(operation);
				statusTest.setTestName("ValidSerial");
				statusTest.setExpectedResult ("201");
				statusTest.setType ("performance");

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
				AsyncContext.Run(async () => await new HTTPCalls().runTest(statusTest, HTTPOperation.POST));
				timer.Stop();

				JObject posted = JObject.FromObject (status);
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

			log.WriteLine ("Test Ended: SyncHTTPDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

			results.Close();
		}

        [Test()]
        public void AsyncHTTPDeviceStatus()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: AsyncHTTPDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
			JObject[] statusPosted = new JObject[TestGlobals.maxReps];

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);

				status.seqNum = i.ToString();
				DeviceStatus operation = new DeviceStatus(TestGlobals.testServer, status);
				Test statusTest = new Test(operation);
				statusTest.setTestName("ValidSerial");
				statusTest.setExpectedResult ("201");
				statusTest.setType ("performance");


				List<Test> tests = new List<Test>();
				tests.Add(statusTest);

                tasks[i] = new HTTPCalls().runTest(statusTest, HTTPOperation.POST);
				testStarted [i] = HTTPCalls.started;
				JObject Json = JObject.FromObject(status);
				statusPosted[i] = Json;

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
				log.WriteLine (statusPosted[seq].ToString ());
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}

            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");


            results.Close();
        }

		[Test()]
		//Multi-client simultaneious scans
		public void MultiClientStatus()
		{
			FileStream stream;
			stream = File.Create(outputFileMultiClientStatus);
			results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: MultiClientDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Server: " + TestGlobals.testServer);




			// Construct started tasks
			Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps, 2];

			DateTime[] testStarted1 = new DateTime[TestGlobals.maxReps];
			DateTime[] testStarted2 = new DateTime[TestGlobals.maxReps];
			JObject[] statusPosted1 = new JObject[TestGlobals.maxReps];
			JObject[] statusPosted2 = new JObject[TestGlobals.maxReps];

			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);

				// client 1
				status.seqNum = i.ToString();
				DeviceStatus operation1 = new DeviceStatus(TestGlobals.testServer, status);

				Test statusTest1 = new Test(operation1);
				statusTest1.setTestName("ValidSerial");
				statusTest1.setExpectedResult ("201");
				statusTest1.setType ("performance");

				tasks[i, 0] = new HTTPCalls().runTest(statusTest1, HTTPOperation.POST);
				testStarted1[i] = HTTPCalls.started;
				JObject Json1 = JObject.FromObject(status);
				statusPosted1[i] = Json1;



				// client 2
				status.seqNum = i.ToString();
				DeviceStatus operation2 = new DeviceStatus(TestGlobals.testServer, status);

				Test statusTest2 = new Test(operation2);
				statusTest2.setTestName("ValidSerial");
				statusTest2.setExpectedResult ("201");
				statusTest2.setType ("performance");

				tasks[i, 1] = new HTTPCalls().runTest(statusTest2, HTTPOperation.POST);
				testStarted2[i] = HTTPCalls.started;
				JObject Json2 = JObject.FromObject(status);
				statusPosted2[i] = Json2;

				Console.WriteLine("Test starting:" + i.ToString());
				Task.WaitAll(tasks[i, 0], tasks[i, 1]);
			}

			log.WriteLine ("Client 1:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 0].Result);

				log.WriteLine ("Client 1 Test " + i + " Started at " + testStarted1 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (statusPosted1 [i].ToString ());
				log.WriteLine ("Test Lasted: " + tasks[i, 0].Result + "ms");
				log.WriteLine ();
			}

			log.WriteLine ("Client 2:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 1].Result);

				log.WriteLine ("Client 2 Test " + i + " Started at " + testStarted2 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (statusPosted2 [i].ToString ());
				log.WriteLine ("Test Lasted: " + tasks[i, 1].Result + "ms");
				if (i < 99) 
				{
					log.WriteLine ();
				}
			}

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: MultiClientDeviceStatus");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

			results.Close();
		}

    }
}
