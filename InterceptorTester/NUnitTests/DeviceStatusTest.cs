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
	public class DeviceStatusTest
	{
		static StreamWriter results;
		static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSDeviceStatusPerformanceTest.csv";
		static string outputFileHTTPAsync = "../../../logs/AsyncHTTPDeviceStatusPerformanceTest.csv";

		static Uri server;
		static string validSerial;
		static string invalidSerial;
		static int delay;

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
				delay = int.Parse(ConfigurationManager.ConnectionStrings["DelayBetweenRuns"].ConnectionString);

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
		public void AsyncHTTPSDeviceStatus()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSAsync);
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
				System.Threading.Thread.Sleep(delay);
				tasks[i] = new HTTPSCalls().runTest(statusTest);
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
		public void AsyncHTTPDeviceStatus()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPAsync);
			results = new StreamWriter(stream);

			DeviceStatus operation = new DeviceStatus(server, status);
			Test statusTest = new Test(operation);
			statusTest.setTestName("ValidSerial");


			List<Test> tests = new List<Test>();
			tests.Add(statusTest);

			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
			timer.Start();

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[maxReps];
			for (int i = 0; i < maxReps; i++)
			{
				System.Threading.Thread.Sleep(delay);
				tasks[i] = new HTTPCalls().runTest(statusTest);
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
	}
}

