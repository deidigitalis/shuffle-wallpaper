namespace UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using ShuffleWallpaper.DTOs;
    using ShuffleWallpaper.Services;
    using Tools;
    using Xunit;

    public class ApplicationServiceTests : IClassFixture<AutoMockerFixture>
    {
        private readonly AutoMockerFixture _mocker;
        private readonly ApplicationService _service;

        public ApplicationServiceTests(AutoMockerFixture mocker)
        {
            _mocker = mocker;
            _mocker.Use<ILogger>(NullLogger.Instance);
            _service = _mocker.CreateInstance<ApplicationService>();
        }

        [Fact]
        public async Task MainAsync_ExistingDImages_RequestTheirDownload()
        {
            // Arrange
            var args = new[] { Directory.GetCurrentDirectory() };

            _mocker.GetMock<IBingService>()
                .Setup(x => x.DownloadMetadataAsync(It.IsAny<int>()))
                .Returns<int>(idx => Task.FromResult(new BingArchiveDto()));

            var images = new[] { Path.Combine(args[0], "image1.jpg"), Path.Combine(args[0], "image1.jpg") };
            _mocker.GetMock<IBingService>()
                .Setup(x => x.DownloadImagesAsync(It.IsAny<BingArchiveDto>(), args[0]))
                .Returns(Task.FromResult<IEnumerable<string>>(images));

            // Act
            await _service.MainAsync(args);

            // Assert
            const int NoIterations = 8;

            _mocker.GetMock<IBingService>()
                .Verify(x => x.DownloadMetadataAsync(It.IsAny<int>()), Times.Exactly(NoIterations));

            _mocker.GetMock<IBingService>()
                .Verify(x => x.DownloadImagesAsync(It.IsAny<BingArchiveDto>(), It.IsAny<string>()), Times.Exactly(NoIterations));
        }
    }
}
