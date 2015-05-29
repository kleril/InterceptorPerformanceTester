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
	public class ICmdTest
	{
		static StreamWriter results;
		public int maxReps;

		static string outputFileHTTPSSync = "../../../logs/SyncHTTPSICmdPerformanceTest.csv";
		static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSICmdPerformanceTest.csv";
		static string outputFileHTTPSync = "../../../logs/SyncHTTPICmdPerformanceTest.csv";
		static string outputFileHTTPAsync = "../../../logs/AsyncHTTPICmdPerformanceTest.csv";


		[TestFixtureSetUp()]
		public void setup()
		{
			/*
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
			*/
			TestGlobals.setup ();
		}



		[Test()]
		public void SyncHTTPSICmd()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSSync);
			results = new StreamWriter(stream);

			for (int i = 0; i < maxReps; i++)
			{
				System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
				ICmd validICmd = new ICmd(TestGlobals.testServer,TestGlobals.validSerial);
				Test validTest = new Test(validICmd);
				validTest.setTestName("ValidSerial");
				List<Test> tests = new List<Test>();
				tests.Add(validTest);

				timer.Start();
				AsyncContext.Run(async () => await new HTTPSCalls().runTest(validTest, HTTPOperation.GET));
				timer.Stop();
				double time = timer.Elapsed.TotalMilliseconds;
				results.WriteLine("Test Time," + time);
				//Verify Server didn't throw up
			}
			results.Close();
		}
		[Test()]
		public void AsyncHTTPSICmd()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSAsync);
			results = new StreamWriter(stream);
			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


			ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
			Test validTest = new Test(validICmd);
			validTest.setTestName("ValidSerial");
			List<Test> tests = new List<Test>();
			tests.Add(validTest);

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[maxReps];
			for (int i = 0; i < maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPSCalls().runTest(validTest, HTTPOperation.GET);
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
		}


		[Test()]
		public void SyncHTTPICmd()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPSync);
			results = new StreamWriter(stream);

			for (int i = 0; i < maxReps; i++)
			{
				System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
				ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
				Test validTest = new Test(validICmd);
				validTest.setTestName("ValidSerial");
				List<Test> tests = new List<Test>();
				tests.Add(validTest);

				timer.Start();
                AsyncContext.Run(async () => await new HTTPCalls().runTest(validTest, HTTPOperation.GET));
				timer.Stop();
				double time = timer.Elapsed.TotalMilliseconds;
				results.WriteLine("Test Time," + time);

				//Verify Server didn't throw up
			}
			results.Close();
		}
		[Test()]
		public void AsyncHTTPICmd()
		{
			FileStream stream;
			stream = File.Create(outputFileHTTPAsync);
			results = new StreamWriter(stream);
			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


			ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
			Test validTest = new Test(validICmd);
			validTest.setTestName("ValidSerial");
			List<Test> tests = new List<Test>();
			tests.Add(validTest);

			// Construct started tasks
			Task<double>[] tasks = new Task<double>[maxReps];
			for (int i = 0; i < maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPCalls().runTest(validTest, HTTPOperation.GET);
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

