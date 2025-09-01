using System.Data.Entity;

namespace DNP.Backbone.Persistencia
{
    public class BackBoneContexto : DbContext
    {
        public BackBoneContexto()
            : base("name=BackBoneContexto")
        {
            //Configuration.ProxyCreationEnabled = false; 
            Configuration.LazyLoadingEnabled = false;
        }

        
    }
}
