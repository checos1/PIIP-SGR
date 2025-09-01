using System.Net;

namespace DNP.Backbone.Comunes.Dto
{
    public class RespuestaViewModel
    {
        public RespuestaViewModel()
        {
            HttpStatusCode = HttpStatusCode.OK;
        }

        public RespuestaViewModel(object data) : this() => Data = data;

        public RespuestaViewModel(string mesajeRetorno) : this() => MensajeRetorno = mesajeRetorno;

        public RespuestaViewModel(object data, HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            Data = data;
        }

        public RespuestaViewModel(object data, string mesajeRetorno, HttpStatusCode httpStatusCode)
        {
            Data = data;
            MensajeRetorno = mesajeRetorno;
            HttpStatusCode = httpStatusCode;
        }

        public RespuestaViewModel(string mesajeRetorno, HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
            MensajeRetorno = mesajeRetorno;
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public object Data { get; set; }

        public string MensajeRetorno { get; set; }

    }
}