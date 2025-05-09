using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section_4
{
    class TaskForAsyncProgramming
    {
        public static void BasicSyntaxExample1()
        {
            Task task = new Task(Work);
            task.Start();
            Console.ReadLine();

            void Work()
            {
                Console.WriteLine("Hello World.");
            }
        }

        public static void BasicSyntaxExample2()
        {
            Task.Run(Work);
            Console.ReadLine();

            void Work()
            {
                Console.WriteLine("Hello World.");
            }
        }

        public static void ReturningValueWithTask()
        {
            var task = Task.Run(Work);
            Console.WriteLine($"Result: {task.Result}");
            Console.ReadLine();

            int Work()
            {
                Thread.Sleep(1000);
                return 1000;
            }
        }

        public async static void Assignment4Async()
        {
            int[] _array = new int[1000];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i;
            }

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

    
            var task1 = Task.Run(() => calculateSum(0, segmentLength)); //Task 1: index 0 to 249 (start = 0, end = 250)
            var task2 = Task.Run(() => calculateSum(segmentLength, segmentLength * 2)); //Task 2: index 250 to 499 (start = 250, end = 500)
            var task3 = Task.Run(() => calculateSum(segmentLength * 2, segmentLength * 3));//Task 3: index 500 to 749 (start = 500, end = 750)
            var task4 = Task.Run(() => calculateSum(segmentLength * 3, _array.Length));//Task 4: index 750 to 999 (start = 750, end = 1000)



            int[] results = await Task.WhenAll(task1, task2, task3, task4);

            int totalSum = results.Sum();
            DateTime endTime = DateTime.Now;

            TimeSpan totalTime = endTime - startTime;


            Console.WriteLine($"Total Sum: {totalSum}");
            Console.WriteLine($"Total Time With Task: {totalTime.TotalSeconds}s");
        }
    }
}
