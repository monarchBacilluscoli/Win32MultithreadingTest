using System.Threading;
using System;

namespace Multithreading
{
    /// <summary>
    /// The synchronization mechanism test
    /// </summary>
    public static class Synchronization
    {
        /// <summary>
        /// A random number generator
        /// </summary>
        public static Random random;
        /// <summary>
        /// Type initialization
        /// </summary>
        static Synchronization()
        {
            random = new Random();
        }
        /// <summary>
        /// The philosopher dining problem - handled by Mutex.WaiAll()
        /// </summary>
        /// <param name="num">The number of philosophers at the dining table</param>
        public static void PhilosopherDining(int num)
        {
            Thread[] ts = new Thread[num];
            Mutex[] ms = new Mutex[num];
            for (int i = 0; i < num; i++)
            {
                ms[i] = new Mutex();
            }
            Boolean isGoOn = true;

            ParameterizedThreadStart philosopherThread = (object index) =>
            {
                int ind = (int)index;
                int rightChopstick = (int)index == num - 1 ? 0 : ind + 1;
                while (isGoOn)
                {
                    Mutex.WaitAll(new Mutex[] { ms[ind], ms[rightChopstick] });
                    System.Console.WriteLine("The {0} philosopher is eating...", ind);
                    Thread.Sleep(random.Next(5000));
                    // Thread.Sleep(3000);
                    ms[ind].ReleaseMutex();
                    ms[rightChopstick].ReleaseMutex();
                }
                return;
            };
            for (int i = 0; i < num; i++)
            {
                ts[i] = new Thread(philosopherThread);
                ts[i].Start(i);
            }
            // ts[1].Priority = ThreadPriority.Highest;

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Volatile.Write(ref isGoOn, false);
            }
            for (int i = 0; i < ts.Length; i++)
            {
                ts[i].Join();
            }
        }
        /// <summary>
        /// A simple semaphore test
        /// </summary>
        /// <returns>The semaphore generated</returns>
        public static Semaphore SemaphoreTest()
        {
            Semaphore s = new Semaphore(3, 3);
            s.WaitOne();
            s.WaitOne();
            s.WaitOne();
            System.Console.WriteLine("wait 成功");
            return s;
        }
        /// <summary>
        /// A thread function which set the event on job done
        /// </summary>
        /// <param name="done">Set the event when job done</param>
        public static void DoSomething(object done)
        {
            Thread.Sleep(Synchronization.random.Next(10) * 1000);
            System.Console.WriteLine("Something done!");
            ((AutoResetEvent)done).Set();
            return;
        }
    }
}