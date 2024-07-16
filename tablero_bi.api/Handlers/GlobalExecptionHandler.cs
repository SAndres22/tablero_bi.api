using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;
using tablero_bi.Application.Common;
using tablero_bi.Application.Interfaces;

namespace tablero_bi.api.Handlers
{
    public class GlobalExecptionHandler : IExceptionHandler
    {
        private readonly ILoggerService _logger;
        public GlobalExecptionHandler(ILoggerService logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                var guid = Guid.NewGuid();
                _logger.LogError($"{guid} : {exception.Message}");

                var error = new Result<string>().Failed(
                    new List<string> { $"Error interno: {guid}" },
                    string.Empty,
                    false,
                    httpContext.Response.StatusCode);

                var jsonError = JsonSerializer.Serialize(error);

                await httpContext.Response.WriteAsync(jsonError);
            }

            return true;
        }
    }
}
