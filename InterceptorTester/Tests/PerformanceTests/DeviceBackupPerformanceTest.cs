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
	public class DeviceBackupPerformanceTest
    {
        static StreamWriter results;
		static StreamWriter log;

        static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceBackupPerformanceTest.csv";
        static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceBackupPerformanceTest.csv";
        static string outputFileHTTPSSync = "../../../logs/SyncHTTPSDeviceBackupPerformanceTest.csv";
        static string outputFileHTTPSync = "../../../logs/SyncHTTPDeviceBackupPerformanceTest.csv";
        static string outputFileMultiClientDeviceBackup = "../../../logs/MultiClientDeviceBackup.csv";

		static string outputFile = "../../../logs/DeviceBackupPerformance.txt";

        [TestFixtureSetUp()]
        public void setup()
        {
            TestGlobals.setup();

			FileStream stream;
			stream = File.Create (outputFile);
			log = new StreamWriter (stream);

			Console.WriteLine (TestGlobals.testServer);

        }

		[TestFixtureTearDown()]
		public void tearDown()
		{
			log.Close();
		}


        [Test()]
        public void SyncHTTPSDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSSync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                BackupItem[] items = new BackupItem[1];
                items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);

                //BackupJSon
                DeviceBackupJSON json = new DeviceBackupJSON();
                json.i = TestGlobals.validSerial;
				json.s = i;
                json.b = items;

                //BackupOperation
                DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

                //Test
                Test backupTest = new Test(operation);
                backupTest.setTestName("ValidSingleBackupItem");
				backupTest.setExpectedResult ("201");
				backupTest.setType ("performance");

                
				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
                AsyncContext.Run(async () => await new HTTPSCalls().runTest(backupTest, HTTPOperation.POST));
                timer.Stop();

				JObject posted = JObject.FromObject (json);
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

			log.WriteLine ("Test Ended: SyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }
			

		[Test()]
        // Valid Single Backup Item
        public void AsyncHTTPSDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
			JObject[] bkPosted = new JObject[TestGlobals.maxReps];


			log.WriteLine ("Test Started: AsyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            BackupItem[] items = new BackupItem[1];
            items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);


            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);

				DeviceBackupJSON json = new DeviceBackupJSON();
				json.i = TestGlobals.validSerial;
				json.b = items;
				json.s = i;

				DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

				Test backupTest = new Test(operation);
				backupTest.setTestName("ValidSingleBackupItem");
				backupTest.setExpectedResult ("201");
				backupTest.setType ("performance");

                tasks[i] = new HTTPSCalls().runTest(backupTest, HTTPOperation.POST);
				testStarted [i] = HTTPSCalls.started;
				JObject Json = JObject.FromObject(json);
				bkPosted[i] = Json;

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
				log.WriteLine (bkPosted[seq].ToString ());
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}

            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        public void SyncHTTPDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                BackupItem[] items = new BackupItem[1];
                items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);

                //BackupJSon
                DeviceBackupJSON json = new DeviceBackupJSON();
                json.i = TestGlobals.validSerial;
				json.s = i;
                json.b = items;

                //BackupOperation
                DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

                //Test
                Test backupTest = new Test(operation);
                backupTest.setTestName("ValidSingleBackupItem");
				backupTest.setExpectedResult ("201");
				backupTest.setType ("performance");

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
				AsyncContext.Run(async () => await new HTTPCalls().runTest(backupTest, HTTPOperation.POST));
				timer.Stop();

				JObject posted = JObject.FromObject (json);
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

			log.WriteLine ("Test Ended: SyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        // Valid Single Backup Item
        public void AsyncHTTPDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: AsyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            BackupItem[] items = new BackupItem[1];
            items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);

            //BackupJSon
            
            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
			JObject[] bkPosted = new JObject[TestGlobals.maxReps];

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                
				DeviceBackupJSON json = new DeviceBackupJSON();
				json.i = TestGlobals.validSerial;
				json.s = i;
				json.b = items;

				//BackupOperation
				DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

				//Test
				Test backupTest = new Test(operation);
				backupTest.setTestName("ValidSingleBackupItem");
				backupTest.setExpectedResult ("201");
				backupTest.setType ("performance");


				System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPCalls().runTest(backupTest, HTTPOperation.POST);
				testStarted [i] = HTTPCalls.started;
				JObject Json = JObject.FromObject(json);
				bkPosted[i] = Json;


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
				log.WriteLine (bkPosted[seq].ToString ());
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}

            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        //Multi-client simultaneious scans
        public void MultiClientDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileMultiClientDeviceBackup);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: MultiClientDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Server: " + TestGlobals.testServer);

            BackupItem[] items = new BackupItem[1];
            items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);
            
            BackupItem[] items2 = new BackupItem[1];
			items2[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);

            
            // Construct started tasks
            Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps, 2];
			DateTime[] testStarted1 = new DateTime[TestGlobals.maxReps];
			DateTime[] testStarted2 = new DateTime[TestGlobals.maxReps];
			JObject[] bkPosted1 = new JObject[TestGlobals.maxReps];
			JObject[] bkPosted2 = new JObject[TestGlobals.maxReps];

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);

				// client 1
				DeviceBackupJSON json = new DeviceBackupJSON();
				json.i = TestGlobals.validSerial;
				json.s = i;
				json.b = items;
				DeviceBackup operation1 = new DeviceBackup(TestGlobals.testServer, json);

				Test backupTest1 = new Test(operation1);
				backupTest1.setTestName("ValidSingleBackupItem");
				backupTest1.setExpectedResult ("201");
				backupTest1.setType ("performance");

				tasks[i, 0] = new HTTPCalls().runTest(backupTest1, HTTPOperation.POST);
				testStarted1[i] = HTTPCalls.started;
				JObject Json = JObject.FromObject(json);
				bkPosted1[i] = Json;



				// client 2
				DeviceBackupJSON json2 = new DeviceBackupJSON();
				json2.i = TestGlobals.validSerial;
				json2.s = i;
				json2.b = items;
				DeviceBackup operation2 = new DeviceBackup(TestGlobals.testServer, json);

				Test backupTest2 = new Test(operation2);
				backupTest2.setTestName("ValidSingleBackupItem");
				backupTest2.setExpectedResult ("201");
				backupTest2.setType ("performance");

                tasks[i, 1] = new HTTPCalls().runTest(backupTest2, HTTPOperation.POST);
                Console.WriteLine("Test starting:" + i.ToString());
				testStarted2[i] = HTTPCalls.started;
				JObject Json2 = JObject.FromObject(json2);
				bkPosted2[i] = Json2;

                Task.WaitAll(tasks[i, 0], tasks[i, 1]);
            }

			log.WriteLine ("Client 1:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 0].Result);

				log.WriteLine ("Client 1 Test " + i + " Started at " + testStarted1 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (bkPosted1 [i].ToString ());
				log.WriteLine ("Test Lasted: " + tasks[i, 0].Result + "ms");
				log.WriteLine ();
			}

			log.WriteLine ("Client 2:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 1].Result);

				log.WriteLine ("Client 2 Test " + i + " Started at " + testStarted2 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Json Posted:");
				log.WriteLine (bkPosted2 [i].ToString ());
				log.WriteLine ("Test Lasted: " + tasks[i, 1].Result + "ms");
				if (i < 99) 
				{
					log.WriteLine ();
				}
			}

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: MultiClientDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

    }
}
