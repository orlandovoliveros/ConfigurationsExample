using ImageLibrary.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageLibrary
{
    public class ThumbnailProcessor : IThumbnailProcessor
    {
        private readonly ILogger<ThumbnailProcessor> _logger;
        private readonly ImageConfiguration _imageConfiguration;

        private ImageSizeConfiguration _thubmnailSizeConfiguration;

        public ThumbnailProcessor(
            ILogger<ThumbnailProcessor> logger, 
            IOptions<ImageConfiguration> options,
            IOptionsMonitor<ImageSizeConfiguration> imageSizeOptions)
        {
            _logger = logger;
            _imageConfiguration = options.Value;
            _thubmnailSizeConfiguration = imageSizeOptions.Get(ImageConfiguration.ConfigurationNameThumbnail);

            imageSizeOptions.OnChange((thumbnailSizeConfiguration, name) =>
            {
                _logger.LogInformation($"***** Configuration has been changed. *****");

                if (name != ImageConfiguration.ConfigurationNameThumbnail)
                {
                    return;
                }

                _thubmnailSizeConfiguration = thumbnailSizeConfiguration;
            });
        }

        public void ProcessImage(string imagePath)
        {
            _logger.LogInformation($"***** Processing: {imagePath} *****");

            _logger.LogInformation($"Compression Level: {_imageConfiguration.CompressionLevel}");
            _logger.LogInformation($"Output Path: {_imageConfiguration.OutputPath}");
            _logger.LogInformation($"Thumbnail Width: {_thubmnailSizeConfiguration.Width}");
            _logger.LogInformation($"Thumbnail File Prefix: {_thubmnailSizeConfiguration.FilePrefix}");
            _logger.LogInformation($"Thumbnail Watermark Text: {_thubmnailSizeConfiguration.WatermarkText}");
        }
    }
}
