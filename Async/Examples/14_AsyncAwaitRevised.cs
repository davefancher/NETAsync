using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static Async.Logger;

namespace Async.Examples
{
    public static class AsyncAwaitRevisedExample
    {
        private static Func<double, Task> DoResize(ImageProcessor processor, string outputFolderPath) =>
            ratio =>
                Task
                    .Factory
                    .StartNew(
                        () =>
                        {
                            LogWarning($"Resizing image to {ratio:P2}");

                            var newImage = processor.ResizeSafe(ratio);

                            LogInfo($"Done resizing image to {ratio:P2}");

                            return newImage;
                        })
                    // Notice the refactoring to hook everything up in DoResize
                    .ContinueWith(
                        a =>
                        {
                            var image = a.Result;
                            var fileName = $"{image.Width}x{image.Height}.jpg";

                            LogWarning($"Saving {fileName} image");

                            Path
                                .Combine(
                                    outputFolderPath,
                                    fileName)
                                            .Apply(image.Save);

                            LogInfo($"Done saving {fileName} image");
                        });

        public static async Task Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                await
                    ratios
                        .Select(DoResize(processor, outputFolderPath))
                        .ToArray()
                        .Pipe(Task.WhenAll);
            }
        }
    }
}
