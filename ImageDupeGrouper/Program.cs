namespace ImageDupeGrouper
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;

    using Shipwreck.Phash;

    public static class Program
    {
        public static void Main(string[] args)
        {
            float requiredScore = float.Parse(args[0], CultureInfo.InvariantCulture);
            var files = new ConcurrentBag<string>();

            for (int i = 1; i < args.Length; i++)
            {
                files.Add(args[i]);
            }

            var images = new ConcurrentBag<PhashedImage>();

            foreach (string file in files)
            {
                images.Add(new PhashedImage(file));
            }

            var groups = new ConcurrentBag<ConcurrentBag<PhashedImage>>();

            foreach (var image in images)
            {
                bool found = false;

                foreach (var group in groups)
                {
                    foreach (var image2 in group)
                    {
                        float score = ImagePhash.GetCrossCorrelation(image.Phash, image2.Phash);

                        if (score < requiredScore)
                        {
                            continue;
                        }

                        group.Add(image);
                        found = true;
                        break;
                    }

                    if (found)
                    {
                        break;
                    }
                }

                if (!found)
                {
                    groups.Add(new ConcurrentBag<PhashedImage>
                    {
                        image
                    });
                }
            }

            foreach (var group in groups)
            {
                foreach (var image in group.OrderByDescending(i => i.PixelCount).ThenByDescending(i => i.FileSize))
                {
                    Console.WriteLine(image.FilePath);
                }

                Console.WriteLine();
            }
        }
    }
}