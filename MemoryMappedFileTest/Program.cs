using System;
using System.IO.MemoryMappedFiles;
using System.Collections;

namespace MemoryMappedFileTest
{
    struct SomeValue
    {
        public SomeValue(int ra, char rc) //? 忘了类如果实现接口，那么调用接口成员会不会装箱了？
        {
            a = ra;
            c = rc;
        }
        public int a;
        public char c;
    }
    class Program
    {
        static void Main(string[] args)
        {
            MemoryMappedFile sharedMemory = MemoryMappedFile.CreateNew("TestSharedMemoryFIle", 1000);
            MemoryMappedViewAccessor sharedMemoryAccessor = sharedMemory.CreateViewAccessor();

            SomeValue sv1 = new SomeValue(1, 'r'), sv2;

            sharedMemoryAccessor.Write(0, ref sv1);
            sharedMemoryAccessor.Read(0, out sv2);

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    break;
                }
            }

            Console.WriteLine("Hello World!");
        }
    }
}
