﻿using System;
using System.Collections.Generic;

namespace VideosApp.Model
{
    public partial class Video
    {
        public Video()
        {
            VideoTags = new HashSet<VideoTag>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int GenreId { get; set; }
        public int TotalSeconds { get; set; }
        public string? StreamingUrl { get; set; }
        public int? ImageId { get; set; }

        public virtual Genre Genre { get; set; } = null!;
        public virtual Image? Image { get; set; }
        public virtual ICollection<VideoTag> VideoTags { get; set; }
    }
}
