using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing
{
    class BasicSyntax
    {
        public static void BasicSyntaxMethod()
        {
            void WriteThreadName()
            {
                for (int i = 0; i <= 100; i++)
                {
                    Console.WriteLine(Thread.CurrentThread.Name);
                    Thread.Sleep(50);
                }
            }
            Thread.CurrentThread.Name = "Main Thread";

            Thread thread1 = new Thread(WriteThreadName);
            thread1.Name = "Worker Thread 1";
            thread1.Priority = ThreadPriority.Highest;

            Thread thread2 = new Thread(WriteThreadName);
            thread2.Name = "Worker Thread 2";
            thread2.Priority = ThreadPriority.Lowest;

            thread1.Start();
            thread2.Start();

            WriteThreadName(); // run main thread
        }
    }
}
