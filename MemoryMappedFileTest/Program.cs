using System;
using System.IO.MemoryMappedFiles;

namespace MemoryMappedFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryMappedFile sharedMemory = MemoryMappedFile.CreateNew("TestSharedMemoryFIle", 1000);

            Console.WriteLine("Hello World!");
        }
    }
}
