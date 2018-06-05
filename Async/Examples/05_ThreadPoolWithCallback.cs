using System;
using System.Drawing;
using System.IO;
using System.Threading;

using static Async.Logger;

namespace Async.Examples
{
    public static class ThreadPoolWithCallbackExample
    {
        private static void DoResize(
            ImageProcessor processor,
            double ratio,
            Action<Image> callback)
        {
            LogWarning($"Resizing image to {ratio:P2}");

            var newImage = processor.ResizeSafe(ratio);

            LogInfo($"Done resizing image to {ratio:P2}");

            callback(newImage);
        }

        private static Action<Image> SaveFile(string outputFolderPath, CountdownEvent countdown) =>
            image =>
            {
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
                    ThreadPool.QueueUserWorkItem(
                        state =>
                        {
                            DoResize(
                                processor,
                                ratio,
                                SaveFile(outputFolderPath, countdown));
                        });
                }

                countdown.Wait();
            }
        }
    }
}
