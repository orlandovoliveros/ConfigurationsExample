using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ImageConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var switchMappings = new Dictionary<string, string>
            {
                { "--ThumbnailWidth", "Thumbnail:Width" },
                { "--cl", "CompressionLevel" },
            };

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args, switchMappings)
                .Build();

            Console.WriteLine("***** Process Image *****");
            Console.WriteLine($"Processing: {args[0]}");

            var imageConfiguration = new ImageConfiguration
            {
                CompressionLevel = 0.99M
            };
            configuration.GetSection("Image").Bind(imageConfiguration);

            ProcessImage("Thumbnail", imageConfiguration.Thumbnail, imageConfiguration.CompressionLevel);
            ProcessImage("Medium", imageConfiguration.Medium, imageConfiguration.CompressionLevel);
            ProcessImage("Large", imageConfiguration.Large, imageConfiguration.CompressionLevel);
        }

        private static void ProcessImage(string imageSize, ImageSizeConfiguration configuration, decimal compressionLevel)
        {
            Console.WriteLine($"{imageSize} Width: {configuration.Width}");
            Console.WriteLine($"{imageSize} File Prefix: {configuration.FilePrefix}");
            Console.WriteLine($"{imageSize} Watermark Text: {configuration.WatermarkText}");
            Console.WriteLine($"{imageSize} Compression Level: {compressionLevel}");
        }
    }

    public class ImageSizeConfiguration
    {
        public int Width { get; set; }

        public string FilePrefix { get; set; }

        public string WatermarkText { get; set; }
    }

    public class ImageConfiguration
    {
        public ImageSizeConfiguration Thumbnail { get; set; }

        public ImageSizeConfiguration Medium { get; set; }

        public ImageSizeConfiguration Large { get; set; }

        public decimal CompressionLevel { get; set; }
    }
}