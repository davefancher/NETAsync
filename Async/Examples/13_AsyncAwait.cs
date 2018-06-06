using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using static Async.Logger;

namespace Async.Examples
{
    public static class AsyncAwaitExample
    {
        private static Task<Image> DoResize(ImageProcessor processor, double ratio) =>
            Task.Factory.StartNew(
                () =>
                {
                    LogWarning($"Resizing image to {ratio:P2}");

                    var newImage = processor.ResizeSafe(ratio);

                    LogInfo($"Done resizing image to {ratio:P2}");

                    return newImage;
                });

        private static Task SaveImage(string outputFolderPath, Image image) =>
            Task.Factory.StartNew(
                () =>
                {
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
                // Notice that we're back to a traditional loop
                foreach(var ratio in ratios)
                {
                    var resizedImage = await DoResize(processor, ratio);
                    await SaveImage(outputFolderPath, resizedImage);
                }
            }
        }
    }
}
