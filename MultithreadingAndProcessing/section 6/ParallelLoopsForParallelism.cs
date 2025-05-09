using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section_6
{
    class ParallelLoopsForParallelism
    {
        public static void example1()
        {
            int[] _array = new int[1000];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i;
            }
            int sum = 0;
            object parallelLock = new object();

            Parallel.For(0, _array.Length, i =>
            {
                //critical section
                lock (parallelLock)
                {
                    sum += _array[i];
                }
            });

            Console.WriteLine($"sum = {sum}");
        }

        public static void DivideAndConquerUsingParallelLoop()
        {
            int[] _array = new int[1000];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i;
            }

            int sum = 0;
            object parallelLock = new object();

            DateTime startTime = DateTime.Now;

            Parallel.For(0, _array.Length, i =>
            {
                lock(parallelLock)
                {
                    sum += _array[i];
                    Thread.Sleep(10);
                }

            });

            DateTime endTime = DateTime.Now;
            TimeSpan totalTime = endTime - startTime;
            Console.WriteLine($"Total Sum: {sum}");
            Console.WriteLine($"Total Time With Parallel Loop: {totalTime.TotalSeconds}s");
        }
    }
}
