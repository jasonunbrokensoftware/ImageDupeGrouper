namespace ImageDupeGrouper
{
    using System.Drawing;
    using System.IO;

    using Shipwreck.Phash;
    using Shipwreck.Phash.Bitmaps;

    public class PhashedImage
    {
        public PhashedImage(string filePath)
        {
            this.FilePath = filePath;
            this.FileSize = new FileInfo(filePath).Length;

            using (var bitmap = (Bitmap)Image.FromFile(filePath))
            {
                this.PixelCount = bitmap.Width * bitmap.Height;
                this.Phash = ImagePhash.ComputeDigest(bitmap.ToLuminanceImage());
            }
        }

        public string FilePath { get; }

        public Digest Phash { get; }

        public int PixelCount { get; }

        public long FileSize { get; }
    }
}