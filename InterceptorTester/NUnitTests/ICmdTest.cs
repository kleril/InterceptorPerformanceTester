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
	/*
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
		*/

		/*
		[Test()]
		public void ValidSerial()
		{

			//Valid
			ICmd validICmd = new ICmd(testServer, validSerial);

			Test validTest = new Test(validICmd);
			validTest.setTestName("ValidSerial");


			List<Test> tests = new List<Test>();
			tests.Add(validTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}

		[Test()]
		public void InvalidSerial()
		{
			//Invalid
			ICmd invalidICmd = new ICmd(testServer, invalidSerial);
			Test invalidTest = new Test(invalidICmd);
			invalidTest.setTestName("BadSerial");

			List<Test> tests = new List<Test>();
			tests.Add(invalidTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));
			foreach (Test nextTest in Program.getTests())
			{
				Console.WriteLine(nextTest.getOperation().getUri());
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}

		[Test()]
		public void MissingSerial()
		{
			//Missing
			ICmd missingICmd = new ICmd(testServer, null);
			Test missingTest = new Test(missingICmd);
			missingTest.setTestName("EmptySerial");


			List<Test> tests = new List<Test>();
			tests.Add(missingTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Console.WriteLine(nextTest.getOperation().getUri());
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}

		[Test()]
		public void NoQuery()
		{
			//Missing
			ICmd missingICmd = new ICmd(testServer, null);
			missingICmd.noQuery = true;
			Test missingTest = new Test(missingICmd);
			missingTest.setTestName("NoQuery");


			List<Test> tests = new List<Test>();
			tests.Add(missingTest);

			AsyncContext.Run(async() => await Program.buildTests(tests));

			foreach (Test nextTest in Program.getTests())
			{
				Console.WriteLine(nextTest.getOperation().getUri());
				Assert.AreEqual(nextTest.getExpectedResult(), nextTest.getActualResult());
			}
		}
	}
*/
}

