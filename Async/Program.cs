using System;
using System.Diagnostics;
using System.IO;
using Async.Examples;

using static Async.Logger;

namespace Async
{
    using DemoRunner = Action<string, string, double[]>;

    class Program
    {
        static readonly string OutputPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "AsyncDemo",
                DateTime.Now.ToString("yyyyMMdd_hhmmss"));

        const string SourceFileName = @"Y:\OneDrive\SkyDrive camera roll\IMG_20180526_113814.jpg";

        static void Main(string[] args)
        {
            Directory.CreateDirectory(OutputPath);

            DemoRunner run =
                //SynchronousExample
                //BadNewThreadExample
                //NewThreadExample
                //ThreadPoolExample
                //ThreadPoolWithCallbackExample
                //APMWithDelegatesExample
                //ParallelInvokeExample
                //ParallelForEachExample
                TaskFactoryExample
                    .Run;

            var sw = new Stopwatch();
            sw.Start();

            run(
                SourceFileName,
                OutputPath,
                new[] { 1.0, 0.75, 0.5, 0.25, 0.1 });

            sw.Stop();

            LogSuccess("-Done-");
            LogSuccess($"Elapsed: {TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)}");

            Console.ReadLine();
        }
    }
}
