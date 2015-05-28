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
        //Globals
        static StreamWriter results;
        public int maxReps;

        static Uri testServer;
        static string validSerial;
        static string invalidSerial;

        static string outputFileSync = "../../../logs/SyncDeviceBackupTestPerformanceTest.csv";
        static string outputFileAsync = "../../../logs/AsyncDeviceBackupTestPerformanceTest.csv";

        [TestFixtureSetUp()]
        public void setup()
        {
            try
            {
                testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
                validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
                invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;

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

        /*
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
            json.i = validSerial;
			json.s = 4;
			json.b = items;

			//BackupOperation
			DeviceBackup operation = new DeviceBackup(testServer, json);

			//Test
			Test backupTest = new Test(operation);
			backupTest.setTestName("ValidSerial");
			List<Test> tests = new List<Test>();
			tests.Add(backupTest);
			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}
         */

		[Test()]
		// Valid Single Backup Item
		public void ValidSingleBackupItemAsync()
		{
            FileStream stream;
            stream = File.Create(outputFileAsync);
            results = new StreamWriter(stream);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

			BackupItem[] items = new BackupItem[1];
			items[0] = getBackupItem(1);

			//BackupJSon
			DeviceBackupJSON json = new DeviceBackupJSON();
            json.i = validSerial;
			json.s = 4;
			json.b = items;

			//BackupOperation
			DeviceBackup operation = new DeviceBackup(testServer, json);

			//Test
			Test backupTest = new Test(operation);
			backupTest.setTestName("ValidSingleBackupItem");

            timer.Start();

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[maxReps];
            for (int i = 0; i < maxReps; i++)
            {
                tasks[i] = new Program().runTest(backupTest);
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
		// No Backup Items
		public void NoBackupItems()
		{
			BackupItem[] items = new BackupItem[0];

			DeviceBackupJSON emptyJson = new DeviceBackupJSON();
            emptyJson.i = validSerial;
			emptyJson.s = 8;
			emptyJson.b = items;

			DeviceBackup emptyOperation = new DeviceBackup(testServer, emptyJson);
			Test emptyTest = new Test(emptyOperation);
			emptyTest.setTestName("NoBackupItems");


			List<Test> tests = new List<Test>();
			tests.Add(emptyTest);
			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
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
            serialJson.i = validSerial;

			DeviceBackup serialOperation = new DeviceBackup(testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("SpecialDynCode");

			List<Test> tests = new List<Test>();
			tests.Add(serialTest);
			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
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
            serialJson.i = validSerial;

			DeviceBackup serialOperation = new DeviceBackup(testServer, serialJson);

			Test serialTest = new Test(serialOperation);
			serialTest.setTestName("NotSpecialDynCode");

			List<Test> tests = new List<Test>();
			tests.Add(serialTest);
			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}
        */

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

	[TestFixture()]
	public class DeviceScanTest
    {
        static StreamWriter results;
        public int maxReps;

        static Uri testServer;
        static string validSerial;
        static string invalidSerial;


        [TestFixtureSetUp()]
        public void setup()
        {
            try
            {
                testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
                validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
                invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;

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
        static string outputFileAsync = "../../../logs/AsyncDeviceScanPerformanceTest.csv";

		// simple scan code

		[Test()]
		// Valid Single Scan
		public void SingleScanSimpleAsync()
		{
            FileStream stream;
            stream = File.Create(outputFileAsync);
            results = new StreamWriter(stream);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

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

            timer.Start();
            // Construct started tasks
            Task<double>[] tasks = new Task<double>[maxReps];
            for (int i = 0; i < maxReps; i++)
            {
                tasks[i] = new Program().runTest(scanTest);
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
	}

	[TestFixture()]
	public class DeviceSettingsTest
    {
        static Uri testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
        static string validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
        static string invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;

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
         */
	}


	[TestFixture()]
	public class DeviceStatusTest
    {
        static StreamWriter results;
        static string outputFileAsync = "../../../logs/DeviceStatusAsyncPerformanceTest.csv";

        static Uri server;
        static string validSerial;
        static string invalidSerial;

        DeviceStatusJSON status;

        public int maxReps;

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
            status.seqNum = "87";
            status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

            try
            {
                server = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
                validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
                invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;

                string testRunsString = ConfigurationManager.ConnectionStrings["TimesToRunTests"].ConnectionString;
                try {
                    maxReps = int.Parse(testRunsString);
                    status.intSerial = validSerial;
                }
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

		[Test()]
		public void ValidSerial()
		{
            FileStream stream;
            stream = File.Create(outputFileAsync);
            results = new StreamWriter(stream);

			DeviceStatus operation = new DeviceStatus(server, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("ValidSerial");


			List<Test> tests = new List<Test>();
			tests.Add (statusTest);

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[maxReps];
            for (int i = 0; i < maxReps; i++)
            {
                tasks[i] = new Program().runTest(statusTest);
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
		public void AlertDataStore()
		{
			DeviceStatusJSON status = new DeviceStatusJSON();
			status.bkupURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceBackup";
			status.callHomeTimeoutData = null;
			status.callHomeTimeoutMode = "0";
			status.capture = "1";
			status.captureMode = "1";
			status.cmdChkInt = "1";
			status.cmdURL = "http://cozumotesttls.cloudapp.net:80/api/iCmd";
			string[] err = new string[3];
			err[0] = "<timestamp>///900///bypassmode";
			err[1] = "wasd";
			err[2] = "qwerty";
			status.errorLog = err;
			status.intSerial = validSerial;
			status.reportURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceStatus";
			status.requestTimeoutValue = "8000";
			status.revId = "52987";
			status.scanURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceScan";
			status.seqNum = "87";
			status.startURL = "http://cozumotesttls.cloudapp.net:80/api/DeviceSetting";

			DeviceStatus operation = new DeviceStatus(server, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("AlertDataStore");


			List<Test> tests = new List<Test>();
			tests.Add(statusTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}

		}
        */
	}
		
    //Tests written
	[TestFixture()]
	public class ICmdTest
    {
		static StreamWriter results;
        public int maxReps;

        static Uri testServer;
        static string validSerial;
        static string invalidSerial;


		[TestFixtureSetUp()]
		public void setup()
		{
		    try
            {
                testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
                validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
                invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;

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

        static string outputFileSync = "../../../logs/SyncICmdPerformanceTest.csv";
        static string outputFileAsync = "../../../logs/AsyncICmdPerformanceTest.csv";


        [Test()]
        public void SynchronousPerformanceTest()
        {
            FileStream stream;
            stream = File.Create(outputFileSync);
            results = new StreamWriter(stream);

            for (int i = 0; i < maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                ICmd validICmd = new ICmd(testServer, validSerial);
                Test validTest = new Test(validICmd);
                validTest.setTestName("ValidSerial");
                List<Test> tests = new List<Test>();
                tests.Add(validTest);

                timer.Start();
                AsyncContext.Run(async () => await new Program().runTest(validTest));
                timer.Stop();
                double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);
                //Verify Server didn't throw up
            }
            results.Close();
        }
        [Test()]
        public void AsynchronousPerformanceTest()
        {
            FileStream stream;
            stream = File.Create(outputFileAsync);
            results = new StreamWriter(stream);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            

            ICmd validICmd = new ICmd(testServer, validSerial);
            Test validTest = new Test(validICmd);
            validTest.setTestName("ValidSerial");
            List<Test> tests = new List<Test>();
            tests.Add(validTest);

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[maxReps];
            for (int i = 0; i < maxReps; i++)
            {
                tasks[i] = new Program().runTest(validTest);
                Console.WriteLine("Test starting:" + i.ToString());
            }
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("All tests initialized, waiting on them to run as async");
            Console.WriteLine("------------------------------------------------------");
            Task.WaitAll(tasks);

            foreach(Task<double> nextResult in tasks)
            {
                results.WriteLine("Test Time," + nextResult.Result);
            }

            results.Close();

            //Verify Server didn't throw up
            //foreach (Test nextTest in Program.getTests()) { Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult()); }
        }

        /*
		[TestFixtureTearDown()]
		public void tearDown()
		{
			
		}
        */
	}
}

