namespace DNP.Backbone.Comunes.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;


    /// <summary>
    /// This class is a pattern for dynamically class properties, 
    /// set all dynamic class properties in array of this class and then use it in class builders functions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FieldMaskDto
    {
        public string FieldName { get; set; }
        public Type FieldType { get; set; }
    }
}