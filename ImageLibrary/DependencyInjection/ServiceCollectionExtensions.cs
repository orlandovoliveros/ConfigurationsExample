using ImageLibrary;
using ImageLibrary.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddImageLibrary(
            this IServiceCollection services,
            IConfiguration configuration,
            ImageConfiguration defaultImageConfiguration,
            Action<ImageSizeConfiguration> configureThumbnailSize)
        {
            var imageConfigurationSection = configuration.GetSection(ImageConfiguration.ConfigurationName);

            services.AddSingleton<IThumbnailProcessor, ThumbnailProcessor>();

            services.AddOptions<ImageConfiguration>()
                .Configure(imageConfiguration =>
                {
                    imageConfiguration.CompressionLevel = defaultImageConfiguration.CompressionLevel;
                })
                .Bind(imageConfigurationSection)
                .ValidateDataAnnotations();

            services.AddOptions<ImageSizeConfiguration>(ImageConfiguration.ConfigurationNameThumbnail)
                .Configure(configureThumbnailSize)
                .Bind(imageConfigurationSection.GetSection(ImageConfiguration.ConfigurationNameThumbnail))
                .ValidateDataAnnotations()
                .PostConfigure(imageSizeConfiguration =>
                {
                    if (imageSizeConfiguration.Width > 96)
                    {
                        imageSizeConfiguration.Width = 42;
                    }
                });

            services.AddSingleton<IValidateOptions<ImageSizeConfiguration>, ValidateImageSizeConfiguration>();

            services.Configure<ImageSizeConfiguration>(ImageConfiguration.ConfigurationNameMedium, imageConfigurationSection.GetSection(ImageConfiguration.ConfigurationNameMedium));
            services.Configure<ImageSizeConfiguration>(ImageConfiguration.ConfigurationNameLarge, imageConfigurationSection.GetSection(ImageConfiguration.ConfigurationNameLarge));

            return services;
        }
    }
}
