namespace DNP.Backbone.Persistencia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pruebaDetalle : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PruebaDetalles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Detalle = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Pruebas", "PruebaDetalleId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Pruebas", "PruebaDetalleId");
            AddForeignKey("dbo.Pruebas", "PruebaDetalleId", "dbo.PruebaDetalles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pruebas", "PruebaDetalleId", "dbo.PruebaDetalles");
            DropIndex("dbo.Pruebas", new[] { "PruebaDetalleId" });
            DropColumn("dbo.Pruebas", "PruebaDetalleId");
            DropTable("dbo.PruebaDetalles");
        }
    }
}
