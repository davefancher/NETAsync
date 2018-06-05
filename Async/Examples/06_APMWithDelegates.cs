using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Runtime.Remoting.Messaging;

using static Async.Logger;

namespace Async.Examples
{
    using ResizeDelegate = Func<ImageProcessor, double, Image>;

    // Asynchronous Programming Model (deprecated)
    public static class APMWithDelegatesExample
    {
        private static Image DoResize(
            ImageProcessor processor,
            double ratio)
        {
            LogWarning($"Resizing image to {ratio:P2}");

            var newImage = processor.ResizeSafe(ratio);

            LogInfo($"Done resizing image to {ratio:P2}");

            return newImage;
        }

        private static AsyncCallback SaveImage(string outputFolderPath, CountdownEvent countdown) =>
            ar =>
            {
                // Pattern described by http://bit.ly/2xGl3BG
                var result = (AsyncResult)ar;
                var caller = (ResizeDelegate)result.AsyncDelegate;

                // The following blocks until the result is ready. Since we're executing
                // inside a function passed to BeginInvoke as a callback the result should
                // already be there.
                var image = caller.EndInvoke(result);

                var fileName = $"{image.Width}x{image.Height}.jpg";

                LogWarning($"Saving {fileName} image");
                var filePath =
                    Path.Combine(
                        outputFolderPath,
                        fileName);
                image.Save(filePath);

                LogInfo($"Done saving {fileName} image");

                countdown.Signal();
            };

        public static void Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                var countdown = new CountdownEvent(ratios.Length);

                foreach (var ratio in ratios)
                {
                    ResizeDelegate resizer = DoResize;

                    resizer.BeginInvoke(
                        processor,
                        ratio,
                        SaveImage(outputFolderPath, countdown),
                        null);
                }

                countdown.Wait();
            }
        }
    }
}
