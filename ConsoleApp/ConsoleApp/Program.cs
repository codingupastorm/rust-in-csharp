using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        [DllImport("RustLibrary.dll", EntryPoint = "process")]
        private static extern void ProcessInRust();

        private static void ProcessCSharp()
        {
            const int threadCount = 10;
            Task[] tasks = new Task[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    int count = 0;
                    for (int j = 0; j < 5_000_000; j++)
                    {
                        count += 1;
                    }
                    Console.WriteLine("C# thread finished with count={0}", count);
                });
            }

            Task.WaitAll(tasks);
        }

        static void Main(string[] args)
        {
            Stopwatch csharpStopwatch = new Stopwatch();
            Stopwatch rustStopwatch = new Stopwatch();

            csharpStopwatch.Start();
            ProcessCSharp();
            csharpStopwatch.Stop();

            rustStopwatch.Start();
            ProcessInRust();
            rustStopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Execution time in C#: " + csharpStopwatch.Elapsed);
            Console.WriteLine("Execution time in Rust: " + rustStopwatch.Elapsed);
        }
    }
}
