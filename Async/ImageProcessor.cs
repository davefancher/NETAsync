using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Image clonedSource;
            lock(SyncRoot)
            {
                clonedSource = (Image)_sourceImage.Clone();
            }

            return new Bitmap(clonedSource, Scale(clonedSource, ratio));
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
