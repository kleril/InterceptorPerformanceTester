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

			log.WriteLine ("Test Started: SyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
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
                json.s = 4;
                json.b = items;

                //BackupOperation
                DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

                //Test
                Test backupTest = new Test(operation);
                backupTest.setTestName("ValidSingleBackupItem");
				backupTest.setExpectedResult ("201");
				backupTest.setType ("performance");

                timer.Start();
                AsyncContext.Run(async () => await new HTTPSCalls().runTest(backupTest, HTTPOperation.POST));
                timer.Stop();
                double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);
                System.Threading.Thread.Sleep(TestGlobals.delay);
                //Verify Server didn't throw up
            }

			log.WriteLine ("Test Ended: SyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ();

            results.Close();
        }

        [Test()]
        // Valid Single Backup Item
        public void AsyncHTTPSDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSAsync);
            results = new StreamWriter(stream);

			log.WriteLine ("Test Started: AsyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            BackupItem[] items = new BackupItem[1];
            items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);

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
			backupTest.setType ("performance");
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

			log.WriteLine ("Test Ended: AsyncHTTPSDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ();

            results.Close();
        }

        [Test()]
        public void SyncHTTPDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSync);
            results = new StreamWriter(stream);

			log.WriteLine ("Test Started: SyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
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
                json.s = 4;
                json.b = items;

                //BackupOperation
                DeviceBackup operation = new DeviceBackup(TestGlobals.testServer, json);

                //Test
                Test backupTest = new Test(operation);
                backupTest.setTestName("ValidSingleBackupItem");
				backupTest.setExpectedResult ("201");
				backupTest.setType ("performance");

                timer.Start();
                AsyncContext.Run(async () => await new HTTPCalls().runTest(backupTest, HTTPOperation.POST));
                timer.Stop();
                double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);
                System.Threading.Thread.Sleep(TestGlobals.delay);
                //Verify Server didn't throw up
            }

			log.WriteLine ("Test Ended: SyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ();

            results.Close();
        }

        [Test()]
        // Valid Single Backup Item
        public void AsyncHTTPDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPAsync);
            results = new StreamWriter(stream);

			log.WriteLine ("Test Started: AsyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            BackupItem[] items = new BackupItem[1];
            items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);

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
			backupTest.setType ("performance");
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

			log.WriteLine ("Test Ended: AsyncHTTPDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ();

            results.Close();
        }

        [Test()]
        //Multi-client simultaneious scans
        public void MultiClientDeviceBackup()
        {
            FileStream stream;
            stream = File.Create(outputFileMultiClientDeviceBackup);
            results = new StreamWriter(stream);

			log.WriteLine ("Test Started: MultiClientDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            BackupItem[] items = new BackupItem[1];
            items[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);
            DeviceBackupJSON json = new DeviceBackupJSON();
            json.i = TestGlobals.validSerial;
            json.s = 4;
            json.b = items;
            DeviceBackup operation1 = new DeviceBackup(TestGlobals.testServer, json);

            Test backupTest1 = new Test(operation1);
            backupTest1.setTestName("ValidSingleBackupItem");
			backupTest1.setExpectedResult ("201");
			backupTest1.setType ("performance");

            BackupItem[] items2 = new BackupItem[1];
            items2[0] = InterceptorTests.DeviceBackupTest.getBackupItem(1);
            DeviceBackupJSON json2 = new DeviceBackupJSON();
            json2.i = TestGlobals.validSerial;
            json2.s = 4;
            json2.b = items;
            DeviceBackup operation2 = new DeviceBackup(TestGlobals.testServer, json);

            Test backupTest2 = new Test(operation2);
            backupTest2.setTestName("ValidSingleBackupItem");
			backupTest2.setExpectedResult ("201");
			backupTest2.setType ("performance");

            // Construct started tasks
            Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps, 2];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i, 0] = new HTTPCalls().runTest(backupTest1, HTTPOperation.POST);
                tasks[i, 1] = new HTTPCalls().runTest(backupTest2, HTTPOperation.POST);
                Console.WriteLine("Test starting:" + i.ToString());
                Task.WaitAll(tasks[i, 0], tasks[i, 1]);
            }

            foreach (Task<double> nextResult in tasks)
            {
                results.WriteLine("Test Time," + nextResult.Result);
            }

			log.WriteLine ("Test Ended: MultiClientDeviceBackup");
			log.WriteLine ("Current Time: " + DateTime.Now);
			log.WriteLine ();

            results.Close();
        }

    }
}
