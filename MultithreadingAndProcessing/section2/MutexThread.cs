using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingAndProcessing.section2
{
    class MutexThread
    {
        public static void GlobalMutexExampleNoMutex()
        {
            string filePath = "counter.txt";

            for (int i = 0; i < 1000; i++)
            {
                int counter = ReadCounter(filePath);
                counter++;
                WriteCounter(filePath, counter);
                
            }
            Console.WriteLine("Processed Finished");
            Console.ReadLine();



            int ReadCounter(string filePath)
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                using(var reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    return string.IsNullOrWhiteSpace(content) ? 0 : int.Parse(content) ;
                }
            }

            void WriteCounter(string filePath, int counter)
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(counter);
                }
            }

        }


        public static void GlobalMutexExampleWithLock()
        {
            string filePath = "counter.txt";
            object lockObj = new object();

            for (int i = 0; i < 1000; i++)
            {
                lock (lockObj)
                {
                    int counter = ReadCounter(filePath);
                    counter++;
                    WriteCounter(filePath, counter);
                    Thread.Sleep(50);
                }
            }
            Console.WriteLine("Processed Finished");
            Console.ReadLine();



            int ReadCounter(string filePath)
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    return string.IsNullOrWhiteSpace(content) ? 0 : int.Parse(content);
                }
            }

            void WriteCounter(string filePath, int counter)
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(counter);
                }
            }

        }

        public static void GlobalMutexExampleWithMutex()
        {
            string filePath = "counter.txt";
            using(var mutex = new Mutex(false, $"GlobalFileMutex:{filePath}"))
            {
                for (int i = 0; i < 1000; i++)
                {
                    mutex.WaitOne();
                    try
                    {
                        int counter = ReadCounter(filePath);
                        counter++;
                        WriteCounter(filePath, counter);
                        Thread.Sleep(50);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }

                }
            }
            Console.WriteLine("Processed Finished");
            Console.ReadLine();



            int ReadCounter(string filePath)
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    return string.IsNullOrWhiteSpace(content) ? 0 : int.Parse(content);
                }
            }

            void WriteCounter(string filePath, int counter)
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(counter);
                }
            }
        }
    }
}
