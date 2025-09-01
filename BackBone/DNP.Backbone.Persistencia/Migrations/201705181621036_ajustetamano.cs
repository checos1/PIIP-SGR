namespace DNP.Backbone.Persistencia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ajustetamano : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PruebaDetalles", "Detalle", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PruebaDetalles", "Detalle", c => c.String());
        }
    }
}
