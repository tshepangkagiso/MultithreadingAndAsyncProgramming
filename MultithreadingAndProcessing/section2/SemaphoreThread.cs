using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section2
{
    class SemaphoreThread
    {
        public static void SemaphoreExample()
        {
            //Request Queue of WebServer
            Queue<string?> RequestQueue = new Queue<string?>();
            using SemaphoreSlim semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);

            Thread monitoringThread = new Thread(MonitorQueue); // thread for monitoring
            monitoringThread.Start();

            Console.WriteLine("Server is running, Type 'exit' to stop");
            while (true) // this runs on main thread
            {
                Console.Write("Submit Request: ");
                string? input = Console.ReadLine();

                if (input?.ToLower() == "exit")
                {
                    Environment.Exit(0);
                }
                RequestQueue.Enqueue(input);//Queueing Requests
            }

            void ProcessInput(string? input)
            {
                try
                {
                    //simulate processing time
                    Thread.Sleep(2000);
                    Console.WriteLine($"Processed Input: {input}");
                }
                finally
                {
                    var prevCount = semaphore.Release();
                    Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} is released, previous count is:{prevCount}");
                }

            }

            void MonitorQueue()
            {
                while (true)
                {
                    if (RequestQueue.Count > 0)
                    {
                        string? input = RequestQueue.Dequeue(); //dequeueing
                        semaphore.Wait();
                        Thread processInputThread = new Thread(() => ProcessInput(input)); //thread for processing
                        processInputThread.Start();
                    }
                    Thread.Sleep(100); //To avoid over loading cpu, because monitorqueue() will be very fast.
                }
            }
        }
    }
}
