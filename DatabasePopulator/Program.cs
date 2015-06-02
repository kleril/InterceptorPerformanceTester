using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePopulator
{
    class Program
    {
        static void Main(string[] args)
        {
            generateScans();
        }

        private static async void generateScans()
        {
            int[] pseudoRandDelay = new int[100];
            int[] pseudoRandBasket = new int[100];
            ConsoleApplication1.HTTPSCalls caller = new ConsoleApplication1.HTTPSCalls();

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(pseudoRandDelay[i] * 10);
                for (int j = 0; j < pseudoRandBasket[i]; j++)
                {
                    System.Threading.Thread.Sleep(pseudoRandDelay[i]);
                    foreach (ConsoleApplication1.Test nextScan in getBasket(j))
                    {
                        await caller.runTest(nextScan, ConsoleApplication1.HTTPOperation.POST);
                    }
                }
            }
        }

        private static List<ConsoleApplication1.Test> getBasket(int basketType)
        {
            List<ConsoleApplication1.Test> basket;

            switch (basketType)
            {
                case 1:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 2:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                case 3:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                case 4:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                case 5:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                case 6:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                case 7:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                case 8:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                case 9:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                case 10:
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
					basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
            }
				
            return basket;
            ConsoleApplication1.HTTPSCalls calls = new ConsoleApplication1.HTTPSCalls();
			calls.runTest ();
        }
    }
}
