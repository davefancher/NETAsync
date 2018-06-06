using System;
using System.Diagnostics;
using System.IO;
//using System.Threading.Tasks;
using Async.Examples;

using static Async.Logger;

namespace Async
{
    using DemoRunner = Action<string, string, double[]>;
    //using AsyncDemoRunner = Func<string, string, double[], Task>;

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
                SynchronousExample
                //BadNewThreadExample
                //NewThreadExample
                //ThreadPoolExample
                //ThreadPoolWithCallbackExample
                //APMWithDelegatesExample
                //ParallelInvokeExample
                //ParallelForEachExample
                //NewTaskExample
                //TaskFactoryExample
                //TaskContinuationExample
                //TaskExceptionsExample
                    .Run;

            //AsyncDemoRunner run =
                //AsyncAwaitExample
                //AsyncAwaitRevisedExample
                //AsyncAwaitCancellationAndExceptionExample
                //    .Run;

            var sw = new Stopwatch();
            sw.Start();

            run(
                SourceFileName,
                OutputPath,
                new[] { 1.0, 0.75, 0.5, 0.25, 0.1 })
                //.Wait()
                ;

            sw.Stop();

            LogSuccess("-Done-");
            LogSuccess($"Elapsed: {TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds)}");

            Console.ReadLine();
        }
    }
}
