using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLibrary.Configuration
{
    public class ImageConfiguration
    {
        public const string ConfigurationName = "Image";
        public const string ConfigurationNameThumbnail = nameof(Thumbnail);
        public const string ConfigurationNameMedium = nameof(Medium);
        public const string ConfigurationNameLarge = nameof(Large);

        [Range(0, 1, ErrorMessage = "Compression level must be between 0 to 1.")]
        public decimal CompressionLevel { get; set; }

        public string OutputPath { get; set; }

        public ImageSizeConfiguration Thumbnail { get; set; }

        public ImageSizeConfiguration Medium { get; set; }

        public ImageSizeConfiguration Large { get; set; }
    }
}
