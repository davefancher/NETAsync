using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static Async.Logger;

namespace Async.Examples
{
    public static class NewTaskExample
    {
        private static Func<double, Action> DoResize(ImageProcessor processor, string outputFolderPath) =>
            ratio =>
                () =>
                {
                    LogWarning($"Resizing image to {ratio:P2}");

                    var newImage = processor.ResizeSafe(ratio);

                    LogInfo($"Done resizing image to {ratio:P2}");

                    var fileName = $"{newImage.Width}x{newImage.Height}.jpg";

                    LogWarning($"Saving {fileName} image");

                    Path
                        .Combine(
                            outputFolderPath,
                            fileName)
                        .Apply(newImage.Save);

                    LogInfo($"Done saving {fileName} image");
                };

        public static void Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                ratios
                    .Select(DoResize(processor, outputFolderPath))
                    .Select(
                        fn =>
                        {
                            // Works but not recommended for most cases
                            var t = new Task(fn);
                            t.Start();

                            return t;
                        })
                    .ToArray()
                    .Apply(Task.WaitAll); // Unlike the Parallel loops, TaskFactory doesn't implictly wait
            }
        }
    }
}
