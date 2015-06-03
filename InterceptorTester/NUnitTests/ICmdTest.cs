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
	public class ICmdTest
	{
		static StreamWriter results;
		public int maxReps;

		static string outputFileHTTPSSync = "../../../logs/SyncHTTPSICmdPerformanceTest.csv";
		static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSICmdPerformanceTest.csv";
		static string outputFileHTTPSync = "../../../logs/SyncHTTPICmdPerformanceTest.csv";
		static string outputFileHTTPAsync = "../../../logs/AsyncHTTPICmdPerformanceTest.csv";
		static string outputFileMultiClientICmd = "../../../logs/MultiClientICmd.csv";

		[TestFixtureSetUp()]
		public void setup()
		{
			TestGlobals.setup();
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

		[Test()]
		//Multi-client simultaneious scans
		public void multiClientICmd()
		{
			FileStream stream;
			stream = File.Create(outputFileMultiClientICmd);
			results = new StreamWriter(stream);

			ICmd validICmd1 = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
			Test validTest1 = new Test(validICmd1);
			validTest1.setTestName("ValidSerial");


			ICmd validICmd2 = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
			Test validTest2 = new Test(validICmd2);
			validTest2.setTestName("ValidSerial");

			// Construct started tasks
			Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps,2];
			for (int i = 0; i < TestGlobals.maxReps; i++)
			{
				System.Threading.Thread.Sleep(TestGlobals.delay);
				tasks[i,0] = new HTTPCalls().runTest(validTest1, HTTPOperation.GET);
				tasks[i,1] = new HTTPCalls().runTest(validTest2, HTTPOperation.GET);
				Console.WriteLine("Test starting:" + i.ToString());
				Task.WaitAll(tasks[i,0], tasks[i,1]);
			}

			foreach (Task<double> nextResult in tasks)
			{
				results.WriteLine("Test Time," + nextResult.Result);
			}

			results.Close();
		}

			


		[Test()]
		public void ValidSerial()
		{
			ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
			Test validTest = new Test(validICmd);
			validTest.setTestName("ValidSerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(validTest, HTTPOperation.GET));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("200", statusCode);
		}


		[Test()]
		public void InvalidSerial()
		{
			ICmd invalidICmd = new ICmd(TestGlobals.testServer, TestGlobals.invalidSerial);
			Test invalidTest = new Test(invalidICmd);
			invalidTest.setTestName("BadSerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(invalidTest, HTTPOperation.GET));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);

		}

		[Test()]
		public void MissingSerial()
		{
			ICmd missingICmd = new ICmd(TestGlobals.testServer, null);
			Test missingTest = new Test(missingICmd);
			missingTest.setTestName("EmptySerial");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(missingTest, HTTPOperation.GET));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("400", statusCode);
		}

		[Test()]
		public void NoQuery()
		{
			ICmd missingICmd = new ICmd(TestGlobals.testServer, null);
			missingICmd.noQuery = true;
			Test missingTest = new Test(missingICmd);
			missingTest.setTestName("NoQuery");

            AsyncContext.Run(async () => await new HTTPSCalls().runTest(missingTest, HTTPOperation.GET));
            string statusCode = HTTPSCalls.result.Key.Property("StatusCode").Value.ToString();
            Assert.AreEqual("404", statusCode);
		}
	}
}

