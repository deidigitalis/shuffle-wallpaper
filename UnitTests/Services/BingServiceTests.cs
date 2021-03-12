namespace UnitTests.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using ShuffleWallpaper.Adapters;
    using ShuffleWallpaper.DTOs;
    using ShuffleWallpaper.Services;
    using Tools;
    using Xunit;
    using static System.FormattableString;

    public class BingServiceTests : IClassFixture<AutoMockerFixture>, IClassFixture<TemporalFilesFixture>
    {
        private readonly AutoMockerFixture _mocker;
        private readonly BingService _service;
        private readonly TemporalFilesFixture _temporalFiles;

        public BingServiceTests(AutoMockerFixture mocker, TemporalFilesFixture temporalFiles)
        {
            _mocker = mocker;
            _temporalFiles = temporalFiles;
            _service = _mocker.CreateInstance<BingService>();
        }

        [Fact]
        public async Task DownloadMetadataAsync_ExistingId_GetsFromAdapter()
        {
            // Arrange
            const int idx = 0;

            _mocker.GetMock<IHttpAdapter>()
                .Setup(x => x.GetFromJsonAsync<BingArchiveDto>("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=10&mkt=en-US"))
                .Returns(Task.FromResult(new BingArchiveDto()));

            // Act
            BingArchiveDto actual = await _service.DownloadMetadataAsync(idx);

            // Assert
            Assert.NotNull(actual);
        }

        [Fact]
        public async Task DownloadImagesAsync_UnexistingImage_DownloadsAndSaves()
        {
            // Arrange
            var metadata = new BingArchiveDto
            {
                Images = new []
                {
                    new BingImagesArchiveDto
                    {
                        Url = "/th?id=myImage001.jpg"
                    }
                }
            };
            string folderPath = "c:\\";

            var streamMock = new Mock<Stream>();
            _mocker.GetMock<IHttpAdapter>()
                .Setup(x => x.GetStreamAsync(new Uri("https://www.bing.com/th?id=myImage001.jpg", UriKind.Absolute), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(streamMock.Object));

            _mocker.GetMock<IFileStreamAdapter>()
                .Setup(x => x.SaveAsync(streamMock.Object, "c:\\myImage001.jpg", It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var images = (await _service.DownloadImagesAsync(metadata, folderPath)).ToList();

            // Assert
            Assert.Single(images);
            Assert.Contains("c:\\myImage001.jpg", images);
        }

        [Fact]
        public async Task DownloadImagesAsync_ExistingImage_DownloadsAndSaves()
        {
            // Arrange
            string filePath = _temporalFiles.CreateTempFile();
            string fileName = Path.GetFileName(filePath);

            var metadata = new BingArchiveDto
            {
                Images = new[]
                {
                    new BingImagesArchiveDto
                    {
                        Url = Invariant(@$"/th?id={fileName}")
                    }
                }
            };
            string folderPath = Path.GetTempPath();

            var streamMock = new Mock<Stream>();
            _mocker.GetMock<IHttpAdapter>()
                .Setup(x => x.GetStreamAsync(new Uri("https://www.bing.com/th?id=myImage001.jpg", UriKind.Absolute), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(streamMock.Object));

            // Act
            var images = (await _service.DownloadImagesAsync(metadata, folderPath)).ToList();

            // Assert
            Assert.Empty(images);
        }
    }
}
