using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ImageLibrary.Configuration
{
    public class ImageSizeConfiguration
    {
        [Range(32, 1024, ErrorMessage = "The width is out of range.")]
        public int Width { get; set; }

        public string FilePrefix { get; set; }

        public string WatermarkText { get; set; }
    }

    public class ValidateImageSizeConfiguration : IValidateOptions<ImageSizeConfiguration>
    {
        public ValidateOptionsResult Validate(string name, ImageSizeConfiguration options)
        { 
            if (name == ImageConfiguration.ConfigurationNameThumbnail && options.Width > 96)
            {
                return ValidateOptionsResult.Fail("From validator: Thumbnail should be 96px or smaller.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
