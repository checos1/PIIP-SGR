namespace DNP.ServiciosWBS.Persistencia.Modelo
{
    public partial class MGAWebContexto
    {
        public MGAWebContexto(string connectionString) : base(connectionString)
        {
            //modelBuilder.Entity<EntityTypeCatalogOption>().Ignore(t => t.);
            //base.OnModelCreating(modelBuilder);
        }
    }
}
