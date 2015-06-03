using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using ConsoleApplication1;

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
            int[] pseudoRandDelay = {12,44,52,122,55,93,21,62,49,25,25,65,83,12,24,66,12,12,182,75,76,712,42,120};
            int[] pseudoRandBasket = {10,4,2,4,5,6,7,8,9,1,5,4,2,1,8,9,7,6,4,10,3,3,5,9,8,1,7,6,5,4,3,10,2,6,2,2,1,2,2,4,2};

            for (int i = 0; i < pseudoRandDelay.Length; i++)
            {
                Console.WriteLine("Set of baskets complete. Sleeping...");
                System.Threading.Thread.Sleep(pseudoRandDelay[i] * 100);
                for (int j = 0; j < pseudoRandBasket.Length; j++)
                {
                    Console.WriteLine("Basket complete. Sleeping...");
                    System.Threading.Thread.Sleep(pseudoRandDelay[i] * 10);
                    Console.WriteLine("Getting next basket");
                    getBasket(pseudoRandBasket[j]);
                    foreach (ConsoleApplication1.Test nextScan in basket)
                    {
                        Console.WriteLine("Posting Scan");
                        AsyncContext.Run(async () => await new ConsoleApplication1.HTTPSCalls().runTest(nextScan, ConsoleApplication1.HTTPOperation.POST));
                        Console.WriteLine("Posted Scan");
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
                    basket.Add(DeviceScanTest.getScan(UpcCode.Laptop11Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Mouse));
                    basket.Add(DeviceScanTest.getScan(UpcCode.LaptopCase11Inch));
                    break;
                case 2:
                    basket.Add(DeviceScanTest.getScan(UpcCode.Laptop13Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Mouse));
                    basket.Add(DeviceScanTest.getScan(UpcCode.LaptopCase13Inch));
                    break;
                case 3:
                    basket.Add(DeviceScanTest.getScan(UpcCode.Printer));
                    basket.Add(DeviceScanTest.getScan(UpcCode.UsbCable));
                    break;
                case 4:
                    basket.Add(DeviceScanTest.getScan(UpcCode.Laptop13Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.LaptopCase13Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Warranty));
                    basket.Add(DeviceScanTest.getScan(UpcCode.ExternalHDD));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Headset));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Mouse));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Keyboard));
                    break;
                case 5:
                    basket.Add(DeviceScanTest.getScan(UpcCode.ExternalHDD));
                    break;
                case 6:
                    basket.Add(DeviceScanTest.getScan(UpcCode.Keyboard));
                    basket.Add(DeviceScanTest.getScan(UpcCode.UsbCable));
                    break;
                case 7:
                    basket.Add(DeviceScanTest.getScan(UpcCode.Speakers));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Laptop11Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Warranty));
                    break;
                case 8:
                    basket.Add(DeviceScanTest.getScan(UpcCode.LaptopCase11Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Laptop11Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Earbuds));
                    break;
                case 9:
                    basket.Add(DeviceScanTest.getScan(UpcCode.HdmiCable));
                    break;
                case 10:
                    basket.Add(DeviceScanTest.getScan(UpcCode.ExternalHDD));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Laptop13Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.LaptopCase13Inch));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Warranty));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Speakers));
                    basket.Add(DeviceScanTest.getScan(UpcCode.Mouse));
                    break;
                default:
                    basket.Add(DeviceScanTest.getScan(UpcCode.ExternalHDD));
                    basket.Add(DeviceScanTest.getScan(UpcCode.UsbCable));
                    break;
            }
				
            return basket;
        }
    }
}
