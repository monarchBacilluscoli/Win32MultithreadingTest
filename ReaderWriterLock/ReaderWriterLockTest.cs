using System;
using System.Threading;

namespace ReaderWriterLock
{
    public class RWLock
    {
        public RWLock(int readerCount)
        {
            m_readerCount = readerCount;
            m_dataLock = new Mutex();
            m_countLock = new SemaphoreSlim(1, 1);
        }
        public void ReaderEnter()
        {
            m_countLock.WaitOne();
            m_readerCount++;
            if (m_readerCount == 1)
            {
                m_dataLock.Wait();
            }
            m_countLock.ReleaseMutex();
        }
        public void WriterEnter()
        {
            m_dataLock.Wait();
        }
        public void ReaderRelease()
        {
            m_countLock.WaitOne();
            m_readerCount--;
            if (m_readerCount == 0)
            {
                m_dataLock.Release();
            }
            m_countLock.ReleaseMutex();
        }
        public void WriterRelease()
        {
            m_dataLock.Release();
        }
        Mutex m_countLock;
        int m_readerCount;
        SemaphoreSlim m_dataLock;
    }
    public static class ReaderWriterLockTest
    {
        static int[] s_data;
        static SemaphoreSlim s_dataMutex;
        static int s_count;
        static Mutex s_countMutex;
        static Random s_rand;
        static bool s_stopFlag;
        public static void Test(int threadSize = 10)
        {
            s_data = new int[threadSize * 2];
            Array.Fill(s_data, 0);
            s_dataMutex = new SemaphoreSlim(1, 1);
            s_count = 0;
            s_countMutex = new Mutex(false);
            s_rand = new Random();
            s_stopFlag = false;
            Thread[] writers = new Thread[threadSize];
            Thread[] readers = new Thread[threadSize];

            for (int i = 0; i < threadSize; i++)
            {
                writers[i] = new Thread(WriterThread);
                writers[i].Start(i * 2);
                readers[i] = new Thread(ReaderThread);
                readers[i].Start(i * 2);
            }

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Volatile.Write(ref s_stopFlag, true);
            }

            for (int i = 0; i < threadSize; i++)
            {
                writers[i].Join();
            }
            return;
        }
        private static void ReaderThread(object index)
        {
            int ind = (int)index;
            while (!s_stopFlag)
            {
                {
                    s_countMutex.WaitOne();
                    s_count++;
                    if (Volatile.Read(ref s_count) == 1)
                    {
                        s_dataMutex.Wait(); // once a reader want, it will wait.
                    }
                    s_countMutex.ReleaseMutex();
                }
                {
                    int temp1 = s_data[ind];
                    Thread.Sleep(500); // to show the read operation is time consuming.
                    int temp2 = s_data[ind + 1];
                    System.Console.WriteLine("{0},{1}", temp1, temp2);
                }
                {
                    s_countMutex.WaitOne();
                    s_count--;
                    if (Volatile.Read(ref s_count) == 0)
                    {
                        s_dataMutex.Release();
                    }
                    s_countMutex.ReleaseMutex();
                }
                Thread.Sleep(s_rand.Next(3000)); // simulate some time-consuming operations on the data, and to give writer a chance
            }
        }

        private static void WriterThread(object index)
        {
            int ind = (int)index;
            while (!s_stopFlag)
            {
                s_dataMutex.Wait();
                Thread.Sleep(100);
                int temp = s_rand.Next(10000);
                s_data[ind] = s_data[ind + 1] = temp;
                s_dataMutex.Release();
                Thread.Sleep(1000);
            }

        }
    }

}