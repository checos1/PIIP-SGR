namespace DNP.ServiciosTransaccional.Persistencia.Modelo
{
    public partial class MGAWebContextoTransaccional
    {
        public MGAWebContextoTransaccional(string connectionString) : base(connectionString)
        {
            //modelBuilder.Entity<EntityTypeCatalogOption>().Ignore(t => t.);
            //base.OnModelCreating(modelBuilder);
        }
    }
}
