using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using ConsoleApplication1;
using System.IO;

namespace DatabasePopulator
{
    class Program
    {
        static List<ConsoleApplication1.Test> basket = new List<ConsoleApplication1.Test>();
		static int basketNum;
		static int scanNum;
		static int totalScan;
		public static string logFile = "../../../logs/scanPopulatorLog.txt";
		static StreamWriter results;

        static void Main(string[] args)
        {
            ConsoleApplication1.TestGlobals.setup();
            generateScans();
        }

        private static async void generateScans()
        {
			int[] pseudoRandDelay = {30};//, 120, 600, 60, 120, 480, 300, 180};
			int[] pseudoRandBasket = { 10};//, 4,2,4,5,6,7,8,9,1,5,4,2,1,8,9,7,6,4,10,3,3,5,9,8,1,7,6,5,4,3,10,2,6,2,2,1,2,2,4,2};

			basketNum = 0;
			totalScan = 0;

			DateTime started = DateTime.Now;

			FileStream stream;
			stream = File.OpenWrite(logFile);
			results = new StreamWriter(stream);


			foreach (int delay in pseudoRandDelay)
            {
				foreach (int basketType in pseudoRandBasket)
                {
					getBasket(basketType);
					Console.WriteLine (DateTime.Now);
					results.WriteLine (DateTime.Now);

					scanNum = 0;

                    foreach (ConsoleApplication1.Test nextScan in basket)
                    {
						Console.WriteLine("Posting Scan");
						results.WriteLine ("Posting Scan");
						AsyncContext.Run(async () => await new ConsoleApplication1.HTTPSCalls().runTest(nextScan, ConsoleApplication1.HTTPOperation.POST));

						Console.WriteLine ("Scan posted:");
						Console.WriteLine (nextScan.getOperation().getJson().ToString ());

						results.WriteLine ("Scan posted:");
						results.WriteLine (nextScan.getOperation ().getJson ().ToString ());


                        Console.WriteLine ("Waiting for next scan...");

						results.WriteLine ("Posted Scan");
						results.WriteLine ("Waiting for next scan...");

						System.Threading.Thread.Sleep (5000);
						scanNum++;
                    }
					basketNum++;
					totalScan += scanNum;
					Console.WriteLine ("Basket complete.");
					Console.WriteLine ("Number of items in this basket: " + scanNum);
					Console.WriteLine ("Total baskets: " + basketNum);
					Console.WriteLine ("Total scans: " + totalScan);
					Console.WriteLine ("Test started at: " + started);
					Console.WriteLine ("Current time: " + DateTime.Now);
					Console.WriteLine ("Waiting for next basket...");

					results.WriteLine ("Basket complete.");
					results.WriteLine ("Number of items in this basket: " + scanNum);
					results.WriteLine ("Total baskets: " + basketNum);
					results.WriteLine ("Total scans: " + totalScan);
					results.WriteLine ("Test started at: " + started);
					results.WriteLine ("Current time: " + DateTime.Now);
					results.WriteLine ("Waiting for next basket...");


					System.Threading.Thread.Sleep(delay * 1000);

					Console.WriteLine ("Getting next basket");
					Console.WriteLine();

					results.WriteLine ("Getting next basket");
					results.WriteLine ();
                }
            }
            Console.WriteLine("Reached end of posts");
			results.WriteLine ("Reached end of posts");
			results.WriteLine ();

			DateTime ended = DateTime.Now;
			TimeSpan testLast = ended - started;
			results.WriteLine ("Summary:");
			results.WriteLine ("Total baskets: " + basketNum);
			results.WriteLine ("Total scans: " + totalScan);
			results.WriteLine ("Test started at " + started);
			results.WriteLine ("Test ended at " + ended);
			results.WriteLine ("Test lasted for " + testLast);

			results.Close ();
        }

        private static List<ConsoleApplication1.Test> getBasket(int basketType)
        {
            basket.Clear();
            ConsoleApplication1.DemoScans scanGen = new ConsoleApplication1.DemoScans();

            switch (basketType)
            {
                case 1:
                    basket.Add(DemoScans.getScan(UpcCode.Laptop11Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Mouse));
                    basket.Add(DemoScans.getScan(UpcCode.LaptopCase11Inch));
                    break;
                case 2:
                    basket.Add(DemoScans.getScan(UpcCode.Laptop13Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Mouse));
                    basket.Add(DemoScans.getScan(UpcCode.LaptopCase13Inch));
                    break;
                case 3:
                    basket.Add(DemoScans.getScan(UpcCode.Printer));
                    basket.Add(DemoScans.getScan(UpcCode.UsbCable));
                    break;
                case 4:
                    basket.Add(DemoScans.getScan(UpcCode.Laptop13Inch));
                    basket.Add(DemoScans.getScan(UpcCode.LaptopCase13Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Warranty));
                    basket.Add(DemoScans.getScan(UpcCode.ExternalHDD));
                    basket.Add(DemoScans.getScan(UpcCode.Headset));
                    basket.Add(DemoScans.getScan(UpcCode.Mouse));
                    basket.Add(DemoScans.getScan(UpcCode.Keyboard));
                    break;
                case 5:
                    basket.Add(DemoScans.getScan(UpcCode.ExternalHDD));
                    break;
                case 6:
                    basket.Add(DemoScans.getScan(UpcCode.Keyboard));
                    basket.Add(DemoScans.getScan(UpcCode.UsbCable));
                    break;
                case 7:
                    basket.Add(DemoScans.getScan(UpcCode.Speakers));
                    basket.Add(DemoScans.getScan(UpcCode.Laptop11Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Warranty));
                    break;
                case 8:
                    basket.Add(DemoScans.getScan(UpcCode.LaptopCase11Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Laptop11Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Earbuds));
                    break;
                case 9:
                    basket.Add(DemoScans.getScan(UpcCode.HdmiCable));
                    break;
                case 10:
                    basket.Add(DemoScans.getScan(UpcCode.ExternalHDD));
                    basket.Add(DemoScans.getScan(UpcCode.Laptop13Inch));
                    basket.Add(DemoScans.getScan(UpcCode.LaptopCase13Inch));
                    basket.Add(DemoScans.getScan(UpcCode.Warranty));
                    basket.Add(DemoScans.getScan(UpcCode.Speakers));
                    basket.Add(DemoScans.getScan(UpcCode.Mouse));
                    break;
                default:
                    basket.Add(DemoScans.getScan(UpcCode.ExternalHDD));
                    basket.Add(DemoScans.getScan(UpcCode.UsbCable));
                    break;
            }
				
            return basket;
        }
    }
}
