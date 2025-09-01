namespace DNP.ServiciosNegocio.Persistencia.Modelo_OnlySP
{
 
    using System;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public partial class ModelOnlySPEntities
    {
        public ModelOnlySPEntities(string connectionString) : base(connectionString)
        {
            //modelBuilder.Entity<EntityTypeCatalogOption>().Ignore(t => t.);
            //base.OnModelCreating(modelBuilder);
        }



    }
}
