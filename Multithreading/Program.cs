using System;
using System.Threading.Tasks;
using System.Threading;

namespace Multithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            if (false)
            {// semaphore test
                var s = Synchronization.SemaphoreTest();
                s.Release(3);
                s.WaitOne();
                s.WaitOne();
                System.Console.WriteLine("wait successfully!");
                return;
            }
            {
                Synchronization.PhilosopherDining(5);
                return;
            }
            {// waitHanle.WaitAny Test - I could not find a method to wait for the thread to finish. I have to use the event
                AutoResetEvent[] jobDone = new AutoResetEvent[]{
                    new AutoResetEvent(false),
                    new AutoResetEvent(false)
                };
                ThreadPool.QueueUserWorkItem(new WaitCallback(Synchronization.DoSomething), jobDone[0]);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Synchronization.DoSomething), jobDone[1]);

                int finishedIndex = -1;
                for (int i = 0; i < 3; i++) // add 3 more jobs to do
                {
                    finishedIndex = WaitHandle.WaitAny(jobDone);
                    System.Console.WriteLine("Waithandle index: " + finishedIndex + " is done! And one more task is running...");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Synchronization.DoSomething), jobDone[finishedIndex]);
                }
                WaitHandle.WaitAll(jobDone);
                return;
            }
        }
    }
}
