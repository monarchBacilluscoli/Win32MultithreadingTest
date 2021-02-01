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
        public override string ToString()
        {
            return String.Format("a: {0}, c: {1}", a, c);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MemoryMappedViewAccessor sharedMemoryAccessor;
            try
            {
                MemoryMappedFile sharedMemory = MemoryMappedFile.OpenExisting("TestSharedMemoryFIle");
                sharedMemoryAccessor = sharedMemory.CreateViewAccessor();

                SomeValue sv2;
                sharedMemoryAccessor.Read(0, out sv2);

                // sharedMemoryAccessor.Write(0, ref sv1);
                Console.WriteLine(sv2);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

        }
    }
}
