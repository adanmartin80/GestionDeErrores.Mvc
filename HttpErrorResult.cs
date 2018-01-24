using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// Envía un error al cliente.
    /// Para usarlo cabecita, en el objeto jqXHR que devuelve cualquier método ajax de jQuery, llamas a la función "fail", que recibe tres parámetros: el jqXHR response, un literal que dice siempre "error" y el literal descriptivo del status code.
    /// En el objeto jqXHR response tienes una propiedad jqXHR.responseJSON que contiene un objeto {message:"", trace:""} el cual indica el ultimo mensaje de la excepción y pila de la excepción.
    /// </summary>
    /// <example>
    ///         $.post('/Home/PruebaExcepcion', function (data) {
	///             console.log(data);
    ///         }).fail(function(jqXHR, err, status)
    ///         {
    ///             console.log(jqXHR.responseJSON.message);
    ///             console.log(jqXHR.responseJSON.trace);
    ///             
    ///             console.log(msgStatus);     // output: "error"
    ///             console.log(status);     // depende del código de error (401, 404, 501...)
    ///         });
    /// </example>
    public class HttpErrorResult : HttpStatusCodeResult
    {
        private Exception _exception;
        private object _object;

        public HttpErrorResult(int statusCode) : base(statusCode) { }
        public HttpErrorResult(System.Net.HttpStatusCode statusCode) : base(statusCode) { }
        public HttpErrorResult(System.Net.HttpStatusCode statusCode, string statusDescription) : base(statusCode, statusDescription) { }
        public HttpErrorResult(int statusCode, string statusDescription) : base(statusCode, statusDescription) { }

        public HttpErrorResult(int statusCode, Exception ex) : base(statusCode) { _exception = ex; }
        public HttpErrorResult(System.Net.HttpStatusCode statusCode, Exception ex) : base(statusCode) { _exception = ex; }
        public HttpErrorResult(System.Net.HttpStatusCode statusCode, string statusDescription, Exception ex) : base(statusCode, statusDescription) { _exception = ex; }
        public HttpErrorResult(int statusCode, string statusDescription, Exception ex) : base(statusCode, statusDescription) { _exception = ex; }

        public HttpErrorResult(int statusCode, object obj) : base(statusCode) { _object = obj; }
        public HttpErrorResult(System.Net.HttpStatusCode statusCode, object obj) : base(statusCode) { _object = obj; }
        public HttpErrorResult(System.Net.HttpStatusCode statusCode, string statusDescription, object obj) : base(statusCode, statusDescription) { _object = obj; }
        public HttpErrorResult(int statusCode, string statusDescription, object obj) : base(statusCode, statusDescription) { _object = obj; }


        public override void ExecuteResult(ControllerContext context)
        {
            var response = context?.RequestContext?.HttpContext?.Response;
            if (response == null)
                return;

            response.ContentType = "application/json";
            response.ContentEncoding = System.Text.Encoding.UTF8;

            response.SuppressContent = false;
            response.StatusCode = base.StatusCode;
            response.StatusDescription = base.StatusDescription;

            response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(_object ?? new { message = _exception?.Message ?? base.StatusDescription , trace = _exception?.StackTrace ?? string.Empty }));

        }
    }
}
