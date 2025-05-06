using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section2
{
    class AutoResetEventSignalThread
    {
        public static void Example1()
        {
            bool isSignaled = false;
            using AutoResetEvent autoResetEvent = new AutoResetEvent(initialState: isSignaled);

            Console.WriteLine("Server is running, type 'go' to proceed");
            string? userInput = string.Empty;

            //Worker Thread
            for(int i = 0; i<3; i++)
            {
                //simulate three threads as workers
                Thread workerThread = new Thread(Worker);
                workerThread.Name = $"Worker_Thread_{i + 1}";
                workerThread.Start();
            }

            //main thread(main thread is the producer) recieves user input and sends signal
            while (true)
            {
                userInput = Console.ReadLine();
                if (userInput?.ToLower() == "go")
                {
                    //send signal
                    autoResetEvent.Set();
                }
            }

            void Worker()
            {
                while (true)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for signal.");

                    //waits here by blocking thread until it gets signal, so code after will work once it gets signal.
                    autoResetEvent.WaitOne();

                    Console.WriteLine($"Signal Recieved: {Thread.CurrentThread.Name} Proceeds.");
                    Thread.Sleep(2000); //simulate process time.
                }
            }
        }
    }
}
