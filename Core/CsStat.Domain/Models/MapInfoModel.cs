﻿namespace CsStat.Domain.Models
{
    public class MapInfoModel
    {
        public string MapName { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
    }

    public class Image
    {
        public string Url { get; set; }
    }
}