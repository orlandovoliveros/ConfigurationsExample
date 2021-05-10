using ImageLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService
{
    public class ImageFileWatcher : IHostedService, IDisposable
    {
        private readonly ILogger<ImageFileWatcher> _logger;
        private readonly IConfiguration _configuration;
        private readonly IThumbnailProcessor _thumbnailProcessor;

        private FileSystemWatcher _watcher;

        public ImageFileWatcher(
            ILogger<ImageFileWatcher> logger, 
            IConfiguration configuration,
            IThumbnailProcessor thumbnailProcessor)
        {
            _logger = logger;
            _configuration = configuration;
            _thumbnailProcessor = thumbnailProcessor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Image file watcher started.");

            var watchPath = _configuration["WatchPath"];

            _logger.LogInformation($"Watch Path: {watchPath}");
            if (!Directory.Exists(watchPath))
            {
                Directory.CreateDirectory(watchPath);
            }

            _watcher = new FileSystemWatcher(watchPath);
            _watcher.Created += OnNewImage;
            _watcher.EnableRaisingEvents = true;

            return Task.CompletedTask;
        }

        private void OnNewImage(object sender, FileSystemEventArgs e)
        {
            //_logger.LogInformation($"New Image: {e.FullPath}");
            _thumbnailProcessor.ProcessImage(e.FullPath);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Image file watcher stopping.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("Image file watcher disposing.");

            _watcher.Dispose();
        }
    }
}
