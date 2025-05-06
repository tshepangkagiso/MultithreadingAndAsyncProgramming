using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section2
{
    class MonitorThread
    {
        public static void BasicMonitor()
        {
            int counter = 0; //shared resource
            object _lock = new object();
            void IncrementCounter()
            {
                for (int i = 0; i < 100000; i++)
                {
                    Monitor.Enter(_lock);
                    try
                    {
                        counter = counter + 1;
                    }
                    finally
                    {
                        Monitor.Exit(_lock);
                    }
                    
                }
            }


            Thread thread1 = new Thread(IncrementCounter);
            Thread thread2 = new Thread(IncrementCounter);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine($"Final counter value: {counter}");
        }

        public static void MonitorAirplaneTickets()
        {
            //Request Queue for server
            Queue<string?> RequestQueue = new Queue<string?>();
            int availableTickets = 10;
            object ticketLock = new object();

            //Monitoring Thread
            Thread monitoringThread = new Thread(MonitorQueue);
            monitoringThread.Start();

            Console.WriteLine("Server is runninng.. \r\n Type 'b' to book \r\n Type 'c' to cancel \r\n Type 'exit' to stop");

            //Main Thread
            while (true)
            {
                string? input = Console.ReadLine();

                if (input?.ToLower() == "exit")
                {
                    Environment.Exit(0);
                }

                //Queueing Requests
                RequestQueue.Enqueue(input);
            }

            void MonitorQueue()
            {
                while (true)
                {
                    if (RequestQueue.Count > 0)
                    {
                        //Dequeueing
                        string? input = RequestQueue.Dequeue();
                        //Processing Thread
                        Thread processThread = new Thread(() => ProcessBooking(input));
                        processThread.Start();
                    }
                    //CPU overloading prevention
                    Thread.Sleep(100);
                }
            }

            void ProcessBooking(string? input)
            {
                if( Monitor.TryEnter(ticketLock, 2000))
                {
                    try
                    {
                        //simulate processing time
                        Thread.Sleep(5000);
                        if (input?.ToLower() == "b")
                        {
                            if (availableTickets > 0)
                            {
                                availableTickets--;
                                Console.WriteLine($"\nYour seat has been booked: {availableTickets} seats are still available.");
                            }
                            else
                            {
                                Console.WriteLine("Seats are fully booked.");
                            }
                        }
                        else if (input?.ToLower() == "c")
                        {
                            if (availableTickets < 10)
                            {
                                availableTickets++;
                                Console.WriteLine($"\nYour booking has been canceled: {availableTickets} seats are still available.");
                            }
                            else
                            {
                                Console.WriteLine("Error: There are no seats to cancel.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Incorrect input... \r\n Type 'b' to book \r\n Type 'c' to cancel \r\n Type 'exit' to stop");
                        }
                    }
                    finally
                    {
                        Monitor.Exit(ticketLock);
                    }
                }
                else
                {
                    Console.WriteLine("Server is busy. Your request was not processed. Please try again.");
                }

            }
        }
    }
}
