using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section_3
{
    class ExceptionHandlingInThreads
    {
        public static void ExceptionThrownByThreadExample()
        {
            Thread? expectionThread = null;
            try
            {
                expectionThread = new Thread(() =>
                {
                    throw new InvalidOperationException("An error occured in this worker thread. This is expected.");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
             
            expectionThread?.Start();
            expectionThread?.Join();
        }


        public static void ExceptionThrownByThreadHandledWellExample()
        {
            Thread expectionThread = new Thread(() =>
            {
                try
                {
                    throw new InvalidOperationException("An error occured in this worker thread. This is expected.");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    Console.ReadLine();
                }
                
            });

            expectionThread.Start();
            expectionThread.Join();

        }


        public static void ExceptionThrownByMutlipleThreadCallStacksHandlingExample()
        {
            List<Exception> exceptions = new List<Exception>();
            object exceptionLock = new object();

            Thread thread1 = new Thread(Work);
            Thread thread2 = new Thread(Work);

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            //main thread

            foreach(var ex in exceptions)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            void Work()
            {
                try
                {
                    throw new InvalidOperationException("An error occured.This is expected.");
                }
                catch(Exception ex)
                {
                    lock (exceptionLock)
                    {
                        exceptions.Add(ex);
                    }
                }
                
            }
        }
    }
}
