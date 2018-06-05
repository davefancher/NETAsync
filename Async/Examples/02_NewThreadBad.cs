using System.IO;
using System.Threading;

using static Async.Logger;

namespace Async.Examples
{
    public static class BadNewThreadExample
    {
        private static void DoResize(ImageProcessor processor, string outputFolderPath, double ratio)
        {
            LogWarning($"Resizing image to {ratio:P2}");

            // Ruh Roh. One of two errors here depending on execution order.
            //
            // 1. Parameter is not valid
            // or
            // 2. Object is currently in use elsewhere.
            //
            // What's wrong?

            using (var newImage = processor.Resize(ratio))
            {
                LogInfo($"Done resizing image to {ratio:P2}");

                LogWarning($"Saving {ratio:P2} image");
                var filePath =
                    Path.Combine(
                        outputFolderPath,
                        $"{newImage.Width}x{newImage.Height}.jpg");
                newImage.Save(filePath);
                LogInfo($"Done saving {ratio:P2} image");
            }
        }

        public static void Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                foreach (var ratio in ratios)
                {
                    var t = new Thread(() => DoResize(processor, outputFolderPath, ratio));
                    t.Start();
                }
            }
        }
    }
}
