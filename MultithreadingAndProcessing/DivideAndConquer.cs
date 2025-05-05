using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing
{
    class DivideAndConquer
    {
        public static void DivideAndConquerNoThreading()
        {
            int[] _array = new int[1000];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i;
            }

            int sum = 0;

            DateTime startTime = DateTime.Now;
            for (int i = 0; i < _array.Length; i++)
            {
                sum += _array[i];
                Thread.Sleep(10);
            }
            DateTime endTime = DateTime.Now;

            TimeSpan totalTime = endTime - startTime;

            Console.WriteLine($"Total Sum: {sum}");
            Console.WriteLine($"Total Time No Threading: {totalTime.TotalSeconds}s");
        }


        public static void DivideAndConquerWithThreading()
        {
            int[] _array = new int[1000];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i;
            }

            int sum1 = 0, sum2 = 0,sum3 = 0, sum4 = 0; // dividing sum into 4 parts

            int numOfThreads = 4; // num of threads we decided to make.
            int segmentLength = _array.Length / numOfThreads; // dividing array into segmants based on num of threads 1000 / 4 = 250

            int calculateSum(int start, int end)
            {
                int sum = 0;
                for (int i = start; i < end; i++)
                {
                    sum += _array[i];
                    Thread.Sleep(10);
                }
                return sum;
            }

            DateTime startTime = DateTime.Now;

            Thread[] threads = new Thread[numOfThreads];
            threads[0] = new Thread(() => sum1 = calculateSum(0, segmentLength)); //Thread 1: index 0 to 249 (start = 0, end = 250)
            threads[1] = new Thread(() => sum2 = calculateSum(segmentLength, segmentLength * 2)); //Thread 2: index 250 to 499 (start = 250, end = 500)
            threads[2] = new Thread(() => sum3 = calculateSum(segmentLength * 2, segmentLength * 3));//Thread 3: index 500 to 749 (start = 500, end = 750)
            threads[3] = new Thread(() => sum4 = calculateSum(segmentLength * 3, _array.Length));//Thread 4: index 750 to 999 (start = 750, end = 1000)
            
            foreach(var thread in threads) { thread.Start(); }

            
            foreach (var thread in threads) { thread.Join(); }// Wait for all threads to complete

            int totalSum = sum1 + sum2 + sum3 + sum4; // combining solution
            DateTime endTime = DateTime.Now;

            TimeSpan totalTime = endTime - startTime;
            

            Console.WriteLine($"Total Sum: {totalSum}");
            Console.WriteLine($"Total Time With Threading: {totalTime.TotalSeconds}s");
        }

    }
}
