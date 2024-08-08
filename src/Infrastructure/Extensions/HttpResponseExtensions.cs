using Application.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions
{
    public static class HttpResponseExtensions
    {
        public static async Task WriteForbiddenResponseAsync(this HttpContext context)
        {
            context.Response.ContentType = AppConstants.ContentType;
            var response = new
            {
                Detail = AppConstants.AuthneticationErrorMessage,
                Title = AppConstants.AuthneticationError
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
