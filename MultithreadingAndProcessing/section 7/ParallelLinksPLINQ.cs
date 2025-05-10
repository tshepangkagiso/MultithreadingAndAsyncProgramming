using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section_7
{
    class ParallelLinksPLINQ
    {
        public static void LINQExample()
        {
            var items = Enumerable.Range(1, 20);

            var evenNumbers = items.Where(i => i % 2 == 0);
            Console.WriteLine($"There are {evenNumbers.Count()} even numbers in this collection.");
            
        }

        public static void PLINQExample()
        {
            var items = Enumerable.Range(1, 2000000);

            var evenNumbers = items.AsParallel().Where(i => i % 2 == 0);
            Console.WriteLine($"There are {evenNumbers.Count()} even numbers in this collection.");
        }
    }
}
