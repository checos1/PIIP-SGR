namespace DNP.Backbone.Web.UI.Code52.i18n
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;

    [ExcludeFromCodeCoverage]
    // Autogenerado o de Configuración.
    public static class LocalizationHelpers {
        public static IHtmlString MetaAcceptLanguage<T>(this HtmlHelper<T> html) {
            var acceptLanguage = HttpUtility.HtmlAttributeEncode(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString(String.Format("<meta name=\"accept-language\" content=\"{0}\">", acceptLanguage));
        }
    }
}