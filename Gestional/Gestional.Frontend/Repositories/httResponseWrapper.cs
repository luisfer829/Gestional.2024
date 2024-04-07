using System.Net;

namespace Gestional.Frontend.Repositories
{
    public class httResponseWrapper<T>
    {
        public httResponseWrapper(T? response, bool error, HttpResponseMessage httResponseMessage)
        {
            Response = response;
            Error = error;
            HttResponseMessage = httResponseMessage;
        }

        public T? Response { get; }
        public bool Error { get; }
        public HttpResponseMessage HttResponseMessage { get; }
        public async Task<string?> GetErrorMessageAsync()
        {
            if(!Error)
            {
                return null;
            }
            var statusCode = HttResponseMessage.StatusCode;
            if(statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado.";
            }
            if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttResponseMessage.Content.ReadAsStringAsync();
            }
            if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Tienes que estar logueado para ejecutar esta operación.";
            }
            if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No Tienes permiso para ejecutar esta operación.";
            }
            return "Ha ocurrido un error inesperado."
        }
    }
}
