namespace UnitTests.Services
{
    using ShuffleWallpaper.Services;
    using Xunit;

    public class QueryStringParserTests
    {
        [Fact]
        public void Parse_WellFormedUri_ReturnsProperties()
        {
            // Arrange
            const string uriString = @"/th?id=OHR.CapePerpetua_EN-US1381606733_1920x1080.jpg&rf=LaDigue_1920x1080.jpg&pid=hp";

            // Act
            var actual = QueryStringParser.Parse(uriString);

            // Assert
            Assert.Equal(3, actual.Count);
            Assert.Contains("id", actual);
            Assert.Contains("rf", actual);
            Assert.Contains("pid", actual);
            Assert.Equal("OHR.CapePerpetua_EN-US1381606733_1920x1080.jpg", actual["id"]);
            Assert.Equal("LaDigue_1920x1080.jpg", actual["rf"]);
            Assert.Equal("hp", actual["pid"]);
        }
    }
}
