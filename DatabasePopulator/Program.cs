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
            int[] pseudoRandDelay = {12,44,52,22,55,93,21,12,4,15,25,35,13,12,24,66,12,12,2,5,76,712,42,120};
            int[] pseudoRandBasket = {1,2,3,4,5,1,2,3,2,3,4,5,6,2,1,3,5,2,1,5,3,3,2,1,1,1,2,4,5,2,2,2,4,2,2,1,2,2,4,2};
            ConsoleApplication1.HTTPSCalls caller = new ConsoleApplication1.HTTPSCalls();

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(pseudoRandDelay[i] * 10);
                for (int j = 0; j < pseudoRandBasket[j]; j++)
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
                    basket.Add(ConsoleApplication1.getScan(3));
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(4));
                    break;
                case 2:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(4));
                case 3:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(1));
                case 4:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(2));
                case 5:
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(4));
                case 6:
                    basket.Add(ConsoleApplication1.getScan(3));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(2));
                case 7:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(3));
                    basket.Add(ConsoleApplication1.getScan(2));
                case 8:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(2));
                case 9:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(4));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(2));
                case 10:
                    basket.Add(ConsoleApplication1.getScan(1));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(2));
                    basket.Add(ConsoleApplication1.getScan(4));
            }

<<<<<<< Updated upstream
            return basket;
=======
            ConsoleApplication1.HTTPSCalls calls = new ConsoleApplication1.HTTPSCalls();
			calls.runTest ();
>>>>>>> Stashed changes
        }
    }
}
