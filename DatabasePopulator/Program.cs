using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace DatabasePopulator
{
    class Program
    {
        static List<ConsoleApplication1.Test> basket = new List<ConsoleApplication1.Test>();

        static void Main(string[] args)
        {
            ConsoleApplication1.TestGlobals.setup();
            generateScans();
        }

        private static async void generateScans()
        {
            int[] pseudoRandDelay = {12,44,52,22,55,93,21,12,4,15,25,35,13,12,24,66,12,12,2,5,76,712,42,120};
            int[] pseudoRandBasket = {1,2,3,4,5,1,2,3,2,3,4,5,6,2,1,3,5,2,1,5,3,3,2,1,1,1,2,4,5,2,2,2,4,2,2,1,2,2,4,2};

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(pseudoRandDelay[i] * 10);
                for (int j = 0; j < pseudoRandBasket.Length; j++)
                {
                    //System.Threading.Thread.Sleep(pseudoRandDelay[i]);
                    getBasket(pseudoRandBasket[j]);
                    foreach (ConsoleApplication1.Test nextScan in basket)
                    {
                        Console.WriteLine("Posting");
                        AsyncContext.Run(async () => await new ConsoleApplication1.HTTPSCalls().runTest(nextScan, ConsoleApplication1.HTTPOperation.POST));
                        Console.WriteLine("Posted");
                    }
                }
            }
            Console.WriteLine("Reached end of posts");
        }

        private static List<ConsoleApplication1.Test> getBasket(int basketType)
        {
            basket.Clear();
            ConsoleApplication1.DeviceScanTest scanGen = new ConsoleApplication1.DeviceScanTest();

            switch (basketType)
            {
                case 1:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(ConsoleApplication1.UpcCode.Laptop11Inch));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 2:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 3:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 4:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 5:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 6:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 7:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    break;
                case 8:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    break;
                case 9:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(4));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    break;
                case 10:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(1));
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(2));
                    break;
                default:
                    basket.Add(ConsoleApplication1.DeviceScanTest.getScan(3));
                    break;
            }
				
            return basket;
        }
    }
}
