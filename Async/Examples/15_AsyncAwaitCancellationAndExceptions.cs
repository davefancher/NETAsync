using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using static Async.Logger;

namespace Async.Examples
{
    public static class AsyncAwaitCancellationAndExceptionExample
    {
        private static Func<double, Task> DoResize(CancellationToken cToken ,ImageProcessor processor, string outputFolderPath) =>
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
                        }, cToken)
                    .ContinueWith(
                        a =>
                        {
                            throw new Exception("Fail");
                            var image = a.Result;
                            var fileName = $"{image.Width}x{image.Height}.jpg";

                            LogWarning($"Saving {fileName} image");

                            Path
                                .Combine(
                                    outputFolderPath,
                                    fileName)
                                            .Apply(image.Save);

                            LogInfo($"Done saving {fileName} image");
                        }, cToken);

        public static async Task Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                var cTokenSource = new CancellationTokenSource();
                //cTokenSource.CancelAfter(TimeSpan.FromSeconds(0.25));
                var cToken = cTokenSource.Token;

                try
                {
                    await
                        ratios
                            .Select(DoResize(cToken, processor, outputFolderPath))
                            .ToArray()
                            .Pipe(Task.WhenAll);
                }
                catch (TaskCanceledException ex)
                {
                    LogError(ex.Message);
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                }
            }
        }
    }
}
