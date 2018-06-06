using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static Async.Logger;

namespace Async.Examples
{
    public static class TaskExceptionsExample
    {
        private static Func<double, Func<Image>> DoResize(ImageProcessor processor, string outputFolderPath) =>
            ratio =>
                () =>
                {
                    LogWarning($"Resizing image to {ratio:P2}");

                    var newImage = processor.ResizeSafe(ratio);

                    LogInfo($"Done resizing image to {ratio:P2}");

                    return newImage;
                };

        private static Action<Task<Image>> SaveImage(string outputFolderPath) =>
            antecedent =>
            {
                // Result blocks if the antecedent task hasn't completed
                var image = antecedent.Result;

                var fileName = $"{image.Width}x{image.Height}.jpg";

                throw new Exception($"Failed to save {fileName}");

                LogWarning($"Saving {fileName} image");

                Path
                    .Combine(
                        outputFolderPath,
                        fileName)
                    .Apply(image.Save);

                LogInfo($"Done saving {fileName} image");
            };

        public static void Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                try
                {
                    ratios
                        .Select(DoResize(processor, outputFolderPath))
                        .Select(
                            fn =>
                                Task
                                    .Factory
                                    .StartNew(fn)
                                    .ContinueWith(SaveImage(outputFolderPath))) // Task parallelism
                        .ToArray()
                        .Apply(Task.WaitAll); // Unlike the Parallel loops, TaskFactory doesn't implictly wait
                }
                catch (AggregateException ex)
                {
                    LogError($"Threw {ex.GetType().Name}");
                    ex
                        .Flatten()
                        .InnerExceptions
                        .ToList()
                        .ForEach(iEx => LogError($"\t{iEx.Message}"));
                }
            }
        }
    }
}
