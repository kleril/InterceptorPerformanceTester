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
using ConsoleApplication1;

namespace InterceptorTester.Tests.PerformanceTests
{
	[TestFixture()]
	public class ICmdPerformanceTest
    {
        static StreamWriter results;
		static StreamWriter log;

        static string outputFileHTTPSSync = "../../../logs/SyncHTTPSICmdPerformanceTest.csv";
        static string outputFileHTTPSAsync = "../../../logs/AsyncHTTPSICmdPerformanceTest.csv";
        static string outputFileHTTPSync = "../../../logs/SyncHTTPICmdPerformanceTest.csv";
        static string outputFileHTTPAsync = "../../../logs/AsyncHTTPICmdPerformanceTest.csv";
        static string outputFileMultiClientICmd = "../../../logs/MultiClientICmd.csv";

		static string outputFile = "../../../logs/ICmdPerformance.txt";

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
        public void SyncHTTPSICmd()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSSync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPSICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);


            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
                Test validTest = new Test(validICmd);
                validTest.setTestName("ValidSerial");
				validTest.setExpectedResult ("200");
				validTest.setType ("performance");
                List<Test> tests = new List<Test>();
                tests.Add(validTest);

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
                timer.Start();
                AsyncContext.Run(async () => await new HTTPSCalls().runTest(validTest, HTTPOperation.GET));
                timer.Stop();
                double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);
				log.WriteLine ("Test Lasted: " + time + "ms");

				if (i < 99) 
				{
					log.WriteLine ();
				}


                //Verify Server didn't throw up
            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;


			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");


            results.Close();
        }

        [Test()]
        public void AsyncHTTPSICmd()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];

			log.WriteLine ("Test Started: AsyncHTTPSICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


            ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
            Test validTest = new Test(validICmd);
            validTest.setTestName("ValidSerial");
			validTest.setExpectedResult ("200");
			validTest.setType ("performance");
            List<Test> tests = new List<Test>();
            tests.Add(validTest);

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPSCalls().runTest(validTest, HTTPOperation.GET);
				testStarted [i] = HTTPSCalls.started;
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
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}
            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;

			log.WriteLine ("Test Ended: AsyncHTTPSICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        public void SyncHTTPICmd()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPSync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: SyncHTTPICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);


            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
                Test validTest = new Test(validICmd);
                validTest.setTestName("ValidSerial");
				validTest.setExpectedResult ("200");
				validTest.setType ("performance");
                List<Test> tests = new List<Test>();
                tests.Add(validTest);

				log.WriteLine ("Test " + i + " Started at " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
				timer.Start();
                AsyncContext.Run(async () => await new HTTPCalls().runTest(validTest, HTTPOperation.GET));
                timer.Stop();
                double time = timer.Elapsed.TotalMilliseconds;
                results.WriteLine("Test Time," + time);

				log.WriteLine ("Test Lasted: " + time + "ms");

				if (i < 99) 
				{
					log.WriteLine ();
				}

                //Verify Server didn't throw up
            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;


			log.WriteLine ("Test Ended: SyncHTTPICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        public void AsyncHTTPICmd()
        {
            FileStream stream;
            stream = File.Create(outputFileHTTPAsync);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: AsyncHTTPICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test Run times: " + TestGlobals.maxReps);
			log.WriteLine ("Server: " + TestGlobals.testServer);

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();


            ICmd validICmd = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
            Test validTest = new Test(validICmd);
            validTest.setTestName("ValidSerial");
			validTest.setExpectedResult ("200");
			validTest.setType ("performance");
            List<Test> tests = new List<Test>();
            tests.Add(validTest);

            // Construct started tasks
            Task<double>[] tasks = new Task<double>[TestGlobals.maxReps];
			DateTime[] testStarted = new DateTime [TestGlobals.maxReps];
            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);
                tasks[i] = new HTTPCalls().runTest(validTest, HTTPOperation.GET);
				testStarted [i] = HTTPCalls.started;
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
				log.WriteLine ("Test Lasted: " + nextResult.Result + "ms");
				seq++;
				if (seq < 99)
				{
					log.WriteLine ();
				}

            }

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;


			log.WriteLine ("Test Ended: AsyncHTTPICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");

            results.Close();
        }

        [Test()]
        //Multi-client simultaneious scans
        public void MultiClientICmd()
        {
            FileStream stream;
            stream = File.Create(outputFileMultiClientICmd);
            results = new StreamWriter(stream);

			DateTime started = DateTime.Now;

			log.WriteLine ("Test Started: MultiClientICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Server: " + TestGlobals.testServer);


            ICmd validICmd1 = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
            Test validTest1 = new Test(validICmd1);
            validTest1.setTestName("ValidSerial");
			validTest1.setExpectedResult ("200");
			validTest1.setType ("performance");


            ICmd validICmd2 = new ICmd(TestGlobals.testServer, TestGlobals.validSerial);
            Test validTest2 = new Test(validICmd2);
            validTest2.setTestName("ValidSerial");
			validTest2.setExpectedResult ("200");
			validTest2.setType ("performance");

            // Construct started tasks
            Task<double>[,] tasks = new Task<double>[TestGlobals.maxReps, 2];
			DateTime[] testStarted1 = new DateTime[TestGlobals.maxReps];
			DateTime[] testStarted2 = new DateTime[TestGlobals.maxReps];

            for (int i = 0; i < TestGlobals.maxReps; i++)
            {
                System.Threading.Thread.Sleep(TestGlobals.delay);
				// client 1
                tasks[i, 0] = new HTTPCalls().runTest(validTest1, HTTPOperation.GET);
				testStarted1[i] = HTTPCalls.started;

				// client 2
                tasks[i, 1] = new HTTPCalls().runTest(validTest2, HTTPOperation.GET);
				testStarted2[i] = HTTPCalls.started;
                Console.WriteLine("Test starting:" + i.ToString());
                Task.WaitAll(tasks[i, 0], tasks[i, 1]);
            }

			log.WriteLine ("Client 1:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 0].Result);

				log.WriteLine ("Client 1 Test " + i + " Started at " + testStarted1 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Test Lasted: " + tasks[i, 0].Result + "ms");
				log.WriteLine ();
			}

			log.WriteLine ("Client 2:");
			for(int i = 0; i < TestGlobals.maxReps; i++) 
			{
				results.WriteLine ("Test Time," + tasks[i, 1].Result);

				log.WriteLine ("Client 2 Test " + i + " Started at " + testStarted2 [i].ToString ("yyyy-MM-dd hh:mm:ss.ffffff"));
				log.WriteLine ("Test Lasted: " + tasks[i, 1].Result + "ms");
				if (i < 99) 
				{
					log.WriteLine ();
				}
			}

			DateTime ended = DateTime.Now;
			TimeSpan lasted = ended - started;


			log.WriteLine ("Test Ended: MultiClientICmd");
			log.WriteLine ("Current Time: " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ffffff"));
			log.WriteLine ("Test lasted: " + lasted);
			log.WriteLine ("\n\n");


            results.Close();
        }
    }
}
