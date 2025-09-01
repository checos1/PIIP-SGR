namespace DNP.Backbone.Comunes.Dto.PowerBI
{
    using System;
    using System.Collections.Generic;

    public class DashboadrsInfoDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public bool? IsReadOnly { get; set; }
        public string EmbedUrl { get; set; }
        public List<DashboardsTileInfoDto> Tiles { get; set; }
    }
}
