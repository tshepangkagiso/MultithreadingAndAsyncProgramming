using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section_3
{
    class ThreadPoolExample
    {
        public static void PrintMaxThreads()
        {
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxIOThreads);
            Console.WriteLine($"Max Worker Threads: {maxWorkerThreads}; Max I/O Threads: {maxIOThreads}");
        }

        public static void PrintAvailableThreads()
        {
            ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableIOThreads);
            ThreadPool.GetMaxThreads(out var maxWorkerThreads, out var maxIOThreads);
            var activeWorkerThreads = maxWorkerThreads -availableWorkerThreads;
            var activeIOThreads = maxIOThreads - availableIOThreads;
            Console.WriteLine($"Active Worker Threads: {activeWorkerThreads}; Active I/O Threads: {activeIOThreads}");
        }

        public static void UseWebServerAssignmentWithThreadPool()
        {
            //Request Queue of WebServer
            Queue<string?> RequestQueue = new Queue<string?>();

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

            //second param of threadpool waitcallback is an object
            void ProcessInput(string? input)
            {
                //simulate processing time
                Thread.Sleep(2000);
                Console.WriteLine($"Processed Input: {input}. Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
            }

            void MonitorQueue()
            {
                while (true)
                {
                    if (RequestQueue.Count > 0)
                    {
                        string? input = RequestQueue.Dequeue(); //dequeueing
                        //Thread processInputThread = new Thread(() => ProcessInput(input)); //thread for processing
                        //processInputThread.Start();

                        //Threadpool
                        //ThreadPool.QueueUserWorkItem(ProcessInput,input);
                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            ProcessInput(input);
                            //Console.WriteLine("Task running on thread pool.");
                        });
                    }
                    Thread.Sleep(100); //To avoid over loading cpu, because monitorqueue() will be very fast.
                }
            }
        }
    }
}
