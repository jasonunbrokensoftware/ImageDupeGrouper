namespace ImageDupeGrouper
{
    using System.Drawing;

    using Shipwreck.Phash;
    using Shipwreck.Phash.Bitmaps;

    public class PhashedImage
    {
        public PhashedImage(string filePath)
        {
            this.FilePath = filePath;

            using (var bitmap = (Bitmap)Image.FromFile(filePath))
            {
                this.Phash = ImagePhash.ComputeDigest(bitmap.ToLuminanceImage());
            }
        }

        public string FilePath { get; }

        public Digest Phash { get; }
    }
}