using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section2
{
    class ManualResetEventSignalThread
    {
        public static void Example1()
        {
            using ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
            Console.WriteLine("Press enter to release all threads...");

            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(Work);
                thread.Name = $"Thread_{i + 1}";
                thread.Start();
            }
            Console.ReadLine();
            manualResetEvent.Set();
            Console.ReadLine();


            void Work()
            {
                string? thread = Thread.CurrentThread.Name;
                Console.WriteLine($"{thread} is waiting for the signal...");
                manualResetEvent.Wait();
                Console.WriteLine($"{thread} has been released.");
            }
        }

        public static void Assignment3TwoWaySignalingProducerConsumerScenario()
        {
            using ManualResetEventSlim manualResetEvent1 = new ManualResetEventSlim(false);
            using ManualResetEventSlim manualResetEvent2 = new ManualResetEventSlim(false);

            Queue<int> foodQueue = new Queue<int>();
            Console.WriteLine("Press 'p' to produce food...");

            //Worker Thread
            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(Workers);
                thread.Name = $"Pig_Thread_{i + 1}";
                thread.Start();
            }

            //Main Thread
            while (true)
            {
                string? input = Console.ReadLine();
                if (input?.ToLower() == "p")
                {
                    ProduceFood();
                    Console.WriteLine("Food Ready...");
                    Thread.Sleep(1000);
                    manualResetEvent1.Set();
                }
                manualResetEvent2.Wait();
                Console.WriteLine("\nPress 'p' to produce food...");
            }

            void Workers()
            {
                while (true)
                {
                    string? threadName = Thread.CurrentThread.Name;
                    Console.WriteLine($"{threadName} is waiting for food...");
                    manualResetEvent1.Wait();

                    ConsumeFood();
                    if (foodQueue.Count == 0)
                    {
                        manualResetEvent2.Set();
                        manualResetEvent1.Reset();
                    }
                    else
                    {
                        Console.WriteLine($"It didnt work. count: {foodQueue.Count}");
                    }
                }
            }

            void ProduceFood()
            {
                //produce food
                for (int i = 0; i < 12; i++)
                {
                    foodQueue.Enqueue(i);
                    Thread.Sleep(400);
                }
            }

            void ConsumeFood()
            {
                while (foodQueue.Count > 0)
                {
                    string? threadName = Thread.CurrentThread.Name;
                    int foodItem = foodQueue.Dequeue();
                    Console.WriteLine($"Food item: {foodItem} consumed by pig: {threadName}");
                    Thread.Sleep(500);
                }
            }
        }


        public static void Assignment3TwoWaySignalingProducerConsumerScenarioSolution()
        {
            Queue<int> queue = new Queue<int>();

            ManualResetEventSlim consumeEvent = new ManualResetEventSlim(false);
            ManualResetEventSlim produceEvent = new ManualResetEventSlim(true);// allows waiting thread to execute from the start

            int consumerCount = 0;
            object lockConsumerCount = new object();

            Thread[] consumerThreads = new Thread[3];

            for (int i = 0; i < 3; i++)
            {
                consumerThreads[i] = new Thread(Consume);
                consumerThreads[i].Name = $"Consumer {i + 1}";
                consumerThreads[i].Start();
            }

            // Producer
            while (true)
            {
                produceEvent.Wait(); // blocks after initial start
                produceEvent.Reset(); // stops the thread from producing many times

                Console.WriteLine("To produce, enter 'p'");
                var input = Console.ReadLine() ?? "";

                if (input.ToLower() == "p")
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        queue.Enqueue(i);
                        Console.WriteLine($"Produced: {i}");
                    }

                    consumeEvent.Set();
                }
            }

            // Consumer's behavior

            void Consume()
            {
                while (true)
                {
                    consumeEvent.Wait();

                    while (queue.TryDequeue(out int item))
                    {
                        // work on the items produced
                        Thread.Sleep(500);
                        Console.WriteLine($"Consumed: {item} from thread: {Thread.CurrentThread.Name}");
                    }

                    lock (lockConsumerCount)
                    {
                        consumerCount++;

                        if (consumerCount == 3)
                        {
                            consumeEvent.Reset();
                            produceEvent.Set();
                            consumerCount = 0;

                            Console.WriteLine("****************");
                            Console.WriteLine("**** More Please! *****");
                            Console.WriteLine("****************");
                        }
                    }
                }
            }
        }
    }
}
