using System;
using System.Diagnostics;
using System.Drawing;

namespace Async
{
    [DebuggerNonUserCode]
    public sealed class ImageProcessor : IDisposable
    {
        private static readonly object SyncRoot = new object();
        public static ImageProcessor FromFile(string fileName) =>
            new ImageProcessor(Image.FromFile(fileName));

        private static int Scale(int original, double ratio) =>
            (int)Math.Ceiling(original * ratio);

        private static Size Scale(Image source, double ratio) =>
            ratio == 1.0
                ? new Size(source.Width, source.Height)
                : new Size(
                    Scale(source.Width, ratio),
                    Scale(source.Height, ratio));

        private readonly Image _sourceImage;

        private ImageProcessor(Image sourceImage)
        {
            _sourceImage = sourceImage;
        }

        public Image Resize(double ratio) =>
            new Bitmap(_sourceImage, Scale(_sourceImage, ratio));

        public Image ResizeSafe(double ratio)
        {
            // NOTE: Image is not thread-safe so we can't asynchronously handle the resize
            //       without some control mechanism
            // TODO: Uncomment for asynchronous examples

            lock (SyncRoot)
            {
                return new Bitmap(_sourceImage, Scale(_sourceImage, ratio));
            }
        }

        #region IDisposable Members
        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _sourceImage.Dispose();
            }

            _disposed = true;
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
