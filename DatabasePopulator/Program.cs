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

        private static void generateScans()
        {
            int[] pseudoRandDelay = new int[100];
            int[] pseudoRandRepeat = new int[100];

            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(pseudoRandDelay[i]);

                scan = ConsoleApplication1.dummyScans.getScan(scanEnum);
            }

            ConsoleApplication1.HTTPSCalls calls = new ConsoleApplication1.HTTPSCalls();
            calls.runTest()
        }
    }
}
