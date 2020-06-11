namespace ImageDupeGrouper
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using Shipwreck.Phash;

    public static class Program
    {
        public static void Main(string[] args)
        {
            float requiredScore = float.Parse(args[0]);
            var files = new ConcurrentBag<string>();

            for (int i = 1; i < args.Length; i++)
            {
                files.Add(args[i]);
            }

            var images = new ConcurrentBag<PhashedImage>();

            Parallel.ForEach(
                files,
                filePath =>
                {
                    images.Add(new PhashedImage(filePath));
                });

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
                foreach (var image in group)
                {
                    Console.WriteLine(image.FilePath);
                }

                Console.WriteLine();
            }
        }
    }
}