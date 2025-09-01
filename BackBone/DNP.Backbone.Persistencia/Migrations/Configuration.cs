using DNP.Backbone.Dominio;

namespace DNP.Backbone.Persistencia.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DNP.Backbone.Persistencia.BackBoneContexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DNP.Backbone.Persistencia.BackBoneContexto context)
        {
            context.PruebaDetalle.AddOrUpdate(new PruebaDetalle
            {
                 Id = Guid.Parse("3777933c-642d-4b32-98f0-7409ae996088"),
                 Detalle = "Detalle"
            });
            context.Formularios.AddOrUpdate(new Prueba
            {
                Id = Guid.Parse("632a1f41-5cb2-4bff-b672-6be28fc060d2"),
                Nombre = "Esto es una prueba",
                PruebaDetalleId = Guid.Parse("3777933c-642d-4b32-98f0-7409ae996088")
            });
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
