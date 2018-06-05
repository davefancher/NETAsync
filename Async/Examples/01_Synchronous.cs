using System.Drawing;
using System.IO;

using static Async.Logger;

namespace Async.Examples
{
    public static class SynchronousExample
    {
        public static void Run(string sourceFileName, string outputFolderPath, double[] ratios)
        {
            using (var processor = ImageProcessor.FromFile(sourceFileName))
            {
                foreach(var ratio in ratios)
                {
                    LogWarning($"Resizing image to {ratio:P2}");
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
            }
        }
    }
}
