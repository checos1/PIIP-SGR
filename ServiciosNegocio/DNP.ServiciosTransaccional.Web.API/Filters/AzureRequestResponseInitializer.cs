using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.IO;
using System.Net.Http;
using System.Web;

namespace DNP.ServiciosTransaccional.Web.API.Filters
{
    public class AzureRequestResponseInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            var requestTelemetry = telemetry as RequestTelemetry;
            if (requestTelemetry != null && HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                if (HttpContext.Current.Response.ContentType.Contains("application/json"))
                {
                    using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
                    {
                        HttpContext.Current.Request.InputStream.Position = 0;
                        string requestBody = reader.ReadToEnd();
                        if (requestTelemetry.Properties.Keys.Contains("requestbody"))
                        {
                            requestTelemetry.Properties["requestbody"] = requestBody;
                        }
                        else
                        {
                            requestTelemetry.Properties.Add("requestbody", requestBody);
                        }
                    }

                    var netHttpRequestMessage = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
                    if (netHttpRequestMessage != null && netHttpRequestMessage.Properties.Keys.Contains("responsejson"))
                    {
                        var responseJson = netHttpRequestMessage.Properties["responsejson"].ToString();
                        if (requestTelemetry.Properties.Keys.Contains("responsebody"))
                        {
                            requestTelemetry.Properties["responsebody"] = responseJson;
                        }
                        else
                        {
                            requestTelemetry.Properties.Add("responsebody", responseJson);
                        }
                    }
                }
            }
        }
    }
}