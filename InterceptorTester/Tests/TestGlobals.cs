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
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace ConsoleApplication1
{
	public static class TestGlobals
	{
		public static Uri testServer;
		public static Uri adminServer;
		public static string validSerial;
		public static string invalidSerial;
		public static int delay;
        public static int maxReps;
        public static string username;
        public static string password;
		public static string orgIdCreated;
		public static string locIdCreated;
        public static string intIdCreated;
        public static string logFile = "../../../logs/testLog.txt";

		public static void setup()
		{
			try
			{
				testServer = new Uri(ConfigurationManager.ConnectionStrings["Server"].ConnectionString);
				adminServer = new Uri(ConfigurationManager.ConnectionStrings["AdminServer"].ConnectionString);
				validSerial = ConfigurationManager.ConnectionStrings["ValidSerial"].ConnectionString;
				invalidSerial = ConfigurationManager.ConnectionStrings["InvalidSerial"].ConnectionString;
				delay = int.Parse(ConfigurationManager.ConnectionStrings["DelayBetweenRuns"].ConnectionString);
                username = ConfigurationManager.ConnectionStrings["Username"].ConnectionString;
                password = ConfigurationManager.ConnectionStrings["Password"].ConnectionString;
				

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
	}
}

