using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using static Async.Logger;

namespace Async.Examples
{
    public static class ParallelForEachExample
    {
        private static Action<double> DoResize(ImageProcessor processor, string outputFolderPath) =>
            ratio =>
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
                Parallel.ForEach(
                    ratios,
                    DoResize(processor, outputFolderPath)); // Void function, No callback, Data parallelism
            }
        }
    }
}
