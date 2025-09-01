using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DNP.Backbone.Web.UI
{
    [ExcludeFromCodeCoverage]
    //Se excluyen los DTO por ser clases POCO. Las clases POCO solo tienen propiedades y no tienen métodos para ser testeados.
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppInsightsHelper.Initialize();
            CambiarClaveDelMachineWebConfig();
        }

        protected void CambiarClaveDelMachineWebConfig()
        {
            var mkType = typeof(MachineKeySection);
            var mkSection = ConfigurationManager.GetSection("system.web/machineKey") as MachineKeySection;
            var rwMethod = mkType.GetMethod("Reset", BindingFlags.NonPublic | BindingFlags.Instance);

            var newConfig = new MachineKeySection();
            newConfig.ApplicationName = mkSection.ApplicationName;
            newConfig.CompatibilityMode = mkSection.CompatibilityMode;
            newConfig.DataProtectorType = mkSection.DataProtectorType;
            newConfig.Validation = mkSection.Validation;

            newConfig.ValidationKey = "899882CBA214231BC17D6BA8D37A7524703EB143CA5C9E4B5FBD0AD70F470DE4E8C66571A890EB2FE5BF081B597C4409FC4F1FAC345BFBE0A0752392868B53AC";
            newConfig.DecryptionKey = "0691FBD7A320769E4770CA642F3A8410D11ACED1671B3F9439F34F437A83E066";
            newConfig.Decryption = "AES";
            newConfig.ValidationAlgorithm = "SHA1";

            rwMethod.Invoke(mkSection, new object[] { newConfig });
        }
    }
}
