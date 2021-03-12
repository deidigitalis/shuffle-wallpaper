namespace ShuffleWallpaper.DTOs
{
    using System.Collections.Generic;

    public class BingArchiveDto
    {
        public ICollection<BingImagesArchiveDto> Images { get; set; }

        public BingToolTips ToolTips { get; set; }
    }
}
