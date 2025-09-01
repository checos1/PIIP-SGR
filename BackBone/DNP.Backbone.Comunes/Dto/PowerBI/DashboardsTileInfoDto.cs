﻿namespace DNP.Backbone.Comunes.Dto.PowerBI
{
    using System;
    public class DashboardsTileInfoDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int? RowSpan { get; set; }
        public int? ColSpan { get; set; }
        public string EmbedUrl { get; set; }
        public string EmbedData { get; set; }
        public string ReportId { get; set; }
        public string DatasetId { get; set; }
    }
}
